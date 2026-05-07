using Microsoft.EntityFrameworkCore;
using News.Domain;
using News.Domain.Models;
using News.Domain.AIModels;
using System;

namespace News.Infrastructure
{
    public class NewsRepository
    {
        private readonly AppDbContext _db;

        public NewsRepository(AppDbContext db)
        {
            _db = db;
        }

        private async Task<T> ExecuteWithRetryAsync<T>(
            Func<Task<T>> action,
            CancellationToken ct,
            int maxRetries = 3,
            int delayMs = 500)
        {
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex) when (IsTransient(ex))
                {
                    if (i == maxRetries - 1)
                        throw;

                    await Task.Delay(delayMs * (i + 1), ct);
                }
            }

            throw new InvalidOperationException("Unreachable");
        }

        private static bool IsTransient(Exception ex)
        {
            return ex is DbUpdateException
                || ex is Npgsql.NpgsqlException
                || ex is TimeoutException;
        }

        public async Task<string[]> FetchTopicsFromDBAsync(CancellationToken ct)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                return await _db.Set<Topic>()
                    .AsNoTracking()
                    .Select(t => t.Name)
                    .ToArrayAsync(ct);
            }, ct);
        }

        public async Task<List<NewsItem>> FetchNewsFromDBAsync(uint takeCount, CancellationToken ct)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                return await _db.NewsItems
                    .AsNoTracking()
                    .Where(n => n.IsSummarized == false)
                    .OrderBy(n => n.PublishedAt)
                    .Take((int)takeCount)
                    .ToListAsync(ct);
            }, ct);
        }

        public async Task<int> SaveSummariesAsync(
            AiSummaryResponse aiResponse,
            CancellationToken ct)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                var ids = aiResponse.Results
                    .Select(x => x.Id)
                    .ToList();

                // Загружаем новости вместе с текущими топиками
                var newsItems = await _db.NewsItems
                    .Include(x => x.NewsTopics)
                    .Where(x => ids.Contains(x.Id))
                    .ToListAsync(ct);

                // Загружаем все доступные топики
                var topics = await _db.Set<Topic>()
                    .ToListAsync(ct);

                int updatedCount = 0;

                foreach (var result in aiResponse.Results)
                {
                    var news = newsItems.FirstOrDefault(x => x.Id == result.Id);

                    if (news == null)
                        continue;

                    news.Summary = result.Summary;
                    news.IsSummarized = true;

                    // Удаляем старые топики
                    news.NewsTopics.Clear();

                    // Добавляем новые
                    foreach (var topicResult in result.Topics)
                    {
                        var topic = topics.FirstOrDefault(t =>
                            t.Name.Equals(
                                topicResult.Name,
                                StringComparison.OrdinalIgnoreCase));

                        if (topic == null)
                            continue;

                        news.NewsTopics.Add(new NewsTopic
                        {
                            NewsItemId = news.Id,
                            TopicId = topic.Id,
                            Score = topicResult.Score
                        });
                    }

                    updatedCount++;
                }

                await _db.SaveChangesAsync(ct);

                return updatedCount;

            }, ct);
        }

        public async Task<int> SaveAsync(IEnumerable<NewsItem> items)
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                var urls = items.Select(x => x.Url).ToList();

                var existingUrls = await _db.NewsItems
                    .Where(x => urls.Contains(x.Url))
                    .Select(x => x.Url)
                    .ToListAsync();

                var newItems = items
                    .Where(x => !existingUrls.Contains(x.Url))
                    .ToList();

                if (newItems.Count == 0)
                    return 0;

                _db.NewsItems.AddRange(newItems);
                return await _db.SaveChangesAsync();
            }, CancellationToken.None);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using News.Domain;
using News.Domain.Models;
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
                    .Where(n => string.IsNullOrWhiteSpace(n.Summary))
                    .OrderBy(n => n.PublishedAt)
                    .Take((int)takeCount)
                    .ToListAsync(ct);
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

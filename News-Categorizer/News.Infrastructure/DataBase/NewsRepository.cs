using Microsoft.EntityFrameworkCore;
using News.Domain;
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

        public async Task<int> SaveAsync(IEnumerable<NewsItem> items)
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
            await _db.SaveChangesAsync();

            return newItems.Count;
        }
    }
}

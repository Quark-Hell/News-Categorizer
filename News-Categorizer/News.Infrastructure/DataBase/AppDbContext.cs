using System;
using Microsoft.EntityFrameworkCore;
using News.Domain;

namespace News.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<NewsItem> NewsItems => Set<NewsItem>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsItem>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.Url).IsUnique();
            });
        }
    }
}

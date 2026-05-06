using Microsoft.EntityFrameworkCore;
using News.Domain;
using News.Domain.Models;
using System;

namespace News.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<NewsItem> NewsItems => Set<NewsItem>();
        public DbSet<Topic> Topics => Set<Topic>();
        public DbSet<NewsTopic> NewsTopics => Set<NewsTopic>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsItem>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.Url)
                      .IsUnique();

                entity.Property(x => x.Url)
                      .IsRequired();

                // связь
                entity.HasMany(x => x.NewsTopics)
                      .WithOne(x => x.NewsItem)
                      .HasForeignKey(x => x.NewsItemId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.HasKey(x => x.Id);
                
                entity.HasIndex(x => x.Name)
                      .IsUnique();

                entity.Property(x => x.Name)
                      .IsRequired();

                entity.HasMany(x => x.NewsTopics)
                      .WithOne(x => x.Topic)
                      .HasForeignKey(x => x.TopicId);
            });


            modelBuilder.Entity<NewsTopic>(entity =>
            {
                entity.HasKey(x => new { x.NewsItemId, x.TopicId });

                entity.Property(x => x.Score)
                      .IsRequired();

                entity.HasIndex(x => new { x.TopicId, x.Score });
            });

            modelBuilder.Entity<Topic>().HasData(
                new Topic { Id = 1, Name = "Technology" },
                new Topic { Id = 2, Name = "Politics" },
                new Topic { Id = 3, Name = "Economy" },
                new Topic { Id = 4, Name = "Science" },
                new Topic { Id = 5, Name = "Entertainment" }
            );
        }
    }
}

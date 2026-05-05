using NewsParser.Aggregator;
using News.Infrastructure;

namespace NewsParser
{
    public class Worker : BackgroundService
    {
        private readonly NewsAggregator _aggregator;
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(NewsAggregator aggregator, ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _aggregator = aggregator;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Parsing started");

                var news = await _aggregator.AggregateAsync(stoppingToken);

                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<NewsRepository>();
                await repo.SaveAsync(news);

                _logger.LogInformation($"Parsed {news.Count} items");

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}

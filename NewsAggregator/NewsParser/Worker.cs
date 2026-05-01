using NewsParser.Aggregator;

namespace NewsParser
{
    public class Worker : BackgroundService
    {
        private readonly NewsAggregator _aggregator;
        private readonly ILogger<Worker> _logger;

        public Worker(NewsAggregator aggregator, ILogger<Worker> logger)
        {
            _aggregator = aggregator;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Parsing started");

                var news = await _aggregator.AggregateAsync(stoppingToken);

                // TODO: сохранить в БД

                _logger.LogInformation($"Parsed {news.Count} items");

                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}

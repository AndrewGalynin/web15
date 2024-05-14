using Microsoft.Extensions.Caching.Memory;

namespace WebApplication6
{
    public class CachService : BackgroundService
    {
        private readonly ILogger<CachService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;

        public CachService(ILogger<CachService> logger, IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var response = await client.GetAsync("https://data.api.xweather.com/observations/seattle,wa?client_id=3ykhUTTjiM0TwTmID2Jj6&client_secret=PhRsxyvHpf3Kw0wYLPCo5ZjpCAeXwXOwDvWGEaYi", stoppingToken);

                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadAsStringAsync();
                    _cache.Set("cachedData", result, TimeSpan.FromMinutes(10));

                    _logger.LogInformation("Data fetched and cached successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while fetching or caching data.");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}

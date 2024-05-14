using System.Text;


namespace WebApplication6
{
    public class CheckService : BackgroundService
    {
        private readonly ILogger<CheckService> _logger;
        private readonly HttpClient _httpClient;

        public CheckService(ILogger<CheckService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    string url = "https://moodle3.chmnu.edu.ua/";

                    var response = await _httpClient.GetAsync(url, stoppingToken);

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"���-������� {url} ��������. ��� �������: {response.StatusCode}");
                        await WriteToLog($"[{DateTime.Now}] {url} - ��������. ��� �������: {response.StatusCode}");
                    }
                    else
                    {
                        _logger.LogWarning($"���-������� {url} ����������. ��� �������: {response.StatusCode}");
                        await WriteToLog($"[{DateTime.Now}] {url} - ����������. ��� �������: {response.StatusCode}");

                    }

                    await WriteToLog($"[{DateTime.Now}] {url} - {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "������� ������� �� ��� �������� ���-�������.");
                }

                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }

        private async Task WriteToLog(string message)
        {
            string logFilePath = "check_log.txt";
            await using var streamWriter = new StreamWriter(logFilePath, true, Encoding.UTF8);
            await streamWriter.WriteLineAsync(message);
        }
    }
}

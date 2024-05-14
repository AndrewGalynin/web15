using System.Net.Mail;
using System.Net;
public class DbEmailService : BackgroundService
{
    private readonly ILogger<DbEmailService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public DbEmailService(ILogger<DbEmailService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DbConnect>();

                    var newRecords = dbContext.clients2.Where(record => record.Check).ToList();

                    foreach (var record in newRecords)
                    {
                        await SendEmailAsync("mailforemail@int.pl", "New Record Added", $"A new record with ID {record.Id} has been added.");

                        record.Check = false;
                    }

                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing records.");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); 
        }

    }
    private async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            using (var client = new SmtpClient("poczta.int.pl", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("mailforemail@int.pl", "itslr131234!");
                client.EnableSsl = true;

                var message = new MailMessage
                {
                    From = new MailAddress("mailforemail@int.pl"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };

                message.To.Add(to);

                await client.SendMailAsync(message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email.");
        }
    }

}

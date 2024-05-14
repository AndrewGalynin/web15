using Quartz;
using System.Net.Mail;
using System.Net;
[DisallowConcurrentExecution]
public class EmailJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {

            var smtpClient = new SmtpClient("poczta.int.pl")
            {
                Port = 587,
                Credentials = new NetworkCredential("mailforemail@int.pl", "itslr131234!"),
                EnableSsl = true,
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("mailforemail@int.pl"),
                Subject = "Test",
                Body = "Mail " + DateTime.Now.ToString("HH:mm:ss"),
            };
            mailMessage.To.Add("marinagorshevskaya@gmail.com");
            await smtpClient.SendMailAsync(mailMessage);

            Console.WriteLine("Success");
        }
        catch (SmtpException smtpEx)
        {
            Console.WriteLine($"Помилка SMTP: {smtpEx.StatusCode} - {smtpEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Загальна помилка: {ex.Message}");
        }

    }

}
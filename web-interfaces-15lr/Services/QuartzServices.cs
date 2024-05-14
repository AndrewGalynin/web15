using Microsoft.AspNetCore.Mvc;
using Quartz.Impl;

[ApiController]
[Route("api/jobs")]
public class QuartzService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var scheduler = new JobScheduler(new StdSchedulerFactory());
        await scheduler.StartAsync();

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }

        await scheduler.StopAsync();
    }

}

using Quartz;
using System;
using System.Threading.Tasks;

public class JobScheduler
{
    private readonly IScheduler _scheduler;

    public JobScheduler(ISchedulerFactory schedulerFactory)
    {
        _scheduler = schedulerFactory.GetScheduler().Result;
    }

    public async Task StartAsync()
    {
        await _scheduler.Start();

        IJobDetail job = JobBuilder.Create<EmailJob>()
            .WithIdentity("myJob", "group1")
            .Build();

        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("myTrigger", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(60)
                .RepeatForever())
            .Build();

        await _scheduler.ScheduleJob(job, trigger);
    }

    public async Task StopAsync()
    {
        await _scheduler.Shutdown();
    }
}

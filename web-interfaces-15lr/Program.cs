using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.AspNetCore;
using Quartz.Impl;
using Quartz.Spi;
using WebApplication6;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    q.AddJob<EmailJob>(j => j
        .WithIdentity("EmailJob")
        .StoreDurably());

    q.AddTrigger(t => t
        .ForJob("EmailJob")
        .WithIdentity("EmailJob-trigger")
        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(16, 1)));
});
builder.Services.AddQuartzServer(options =>
{
    options.WaitForJobsToComplete = true;
});

builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

builder.Services.AddScoped<EmailJob>();
builder.Services.AddHostedService<QuartzService>();
builder.Services.AddHostedService<CheckService>();
builder.Services.AddHostedService<DbEmailService>();

builder.Services.AddDbContext<DbConnect>(options =>
    options.UseSqlServer("Server=LAPTOP-H7RBCDUO\\SQLEXPRESS;Database=Horshevska309;Trusted_Connection=true;Encrypt=True;TrustServerCertificate=true;"));

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddHostedService<CachService>();
builder.Services.AddSignalR();
var app = builder.Build();

await app.RunAsync();
using System;
using BackupService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
    .CreateLogger();

var builder = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices(services =>
    {
        services.AddHttpClient<IBackupOrchestrator, SimpleBackupOrchestrator>(c =>
        {
            c.BaseAddress = new Uri("http://localhost:7071/");
        });
    });

var host = builder.Build();

if (args.Contains("--sync"))
{
    var orchestrator = host.Services.GetRequiredService<IBackupOrchestrator>();
    var logger = host.Services.GetRequiredService<ILogger<BackupService.BackupService>>();
    await orchestrator.SyncAsync();
    logger.LogInformation("Sync completed");
}

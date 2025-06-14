using System;
using BackupService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
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
    await orchestrator.SyncAsync();
    Console.WriteLine("Sync completed");
}

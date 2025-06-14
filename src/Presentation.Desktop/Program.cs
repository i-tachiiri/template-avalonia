using System;
using Avalonia;
using Serilog.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;

namespace Presentation.Desktop;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
            .WriteTo.EventLog("MyApp", manageEventSource: true, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
            .CreateLogger();

        var host = Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureServices((context, services) =>
            {
                var infra = Assembly.Load("Infrastructure");
                var extType = infra.GetType("Infrastructure.ServiceCollectionExtensions");
                var method = extType?.GetMethod("AddInfrastructure");
                method?.Invoke(null, new object[] { services, "Data Source=app.db" });
            })
            .Build();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}

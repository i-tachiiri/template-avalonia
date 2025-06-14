using System;
using System.Reflection;
using Avalonia;
using Serilog.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Core.Application;
using Core.Application.Commands;

namespace Presentation.Desktop;

class Program
{
    public static IHost AppHost { get; private set; } = default!;

    [STAThread]
    public static void Main(string[] args)
    {
        var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7);

        if (OperatingSystem.IsWindows())
        {
            loggerConfig = loggerConfig.WriteTo.EventLog("MyApp", manageEventSource: false, restrictedToMinimumLevel: LogEventLevel.Warning);
        }

        Log.Logger = loggerConfig.CreateLogger();

        AppHost = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration.GetConnectionString("Sqlite") ?? "Data Source=app.db";
                var infra = Assembly.Load("Infrastructure");
                var extType = infra.GetType("Infrastructure.ServiceCollectionExtensions");
                var method = extType?.GetMethod("AddInfrastructure");
                method?.Invoke(null, new object[] { services, connectionString });

                services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateNoteHandler>());
                services.AddTransient<INoteService, NoteService>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<MainWindow>();
            })
            .Build();

        AppHost.Start();
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}

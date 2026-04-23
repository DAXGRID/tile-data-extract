using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.Text.Json;

namespace TileDataExtract;

internal static class Program
{
    internal static async Task Main(string[] args)
    {
        using var serviceProvider = BuildServiceProvider(args.Length > 0 ? args[0] : "appsettings.json");
        var startup = serviceProvider.GetService<Startup>();

        if (startup is not null)
            await startup.StartAsync().ConfigureAwait(false);
        else
            throw new ArgumentNullException(nameof(startup));
    }

    private static ServiceProvider BuildServiceProvider(string appsettingsPath)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console(new CompactJsonFormatter())
            .CreateLogger();

        var settingsJson = JsonDocument.Parse(File.ReadAllText(appsettingsPath))
            .RootElement.GetProperty("settings").ToString();

        var settings = JsonSerializer.Deserialize<Settings>(settingsJson) ??
            throw new ArgumentException("Could not deserialize appsettings into settings.");

        return new ServiceCollection()
            .AddSingleton<Settings>(settings)
            .AddLogging(logging =>
            {
                logging.AddSerilog(logger, true);
            })
            .AddSingleton<Startup>()
            .BuildServiceProvider();
    }
}

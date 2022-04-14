using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace TileDataExtract;

internal static class Program
{
    internal static async Task Main()
    {
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, ".env");
        DotEnv.Load(dotenv);

        using var serviceProvider = BuildServiceProvider();
        var startup = serviceProvider.GetService<Startup>();

        if (startup is not null)
            await startup.StartAsync().ConfigureAwait(false);
        else
            throw new ArgumentNullException(nameof(startup));
    }

    private static ServiceProvider BuildServiceProvider()
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console(new CompactJsonFormatter())
            .CreateLogger();

        return new ServiceCollection()
            .AddSingleton<IConfiguration>(config)
            .AddLogging(logging =>
            {
                logging.AddSerilog(logger, true);
            })
            .AddSingleton<Startup>()
            .BuildServiceProvider();
    }
}

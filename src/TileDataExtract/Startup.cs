using Microsoft.Extensions.Logging;

namespace TileDataExtract;

internal class Startup
{
    private readonly ILogger<Startup> _logger;

    public Startup(ILogger<Startup> logger)
    {
        _logger = logger;
    }

    public async Task Start()
    {
        _logger.LogInformation($"Starting {nameof(TileDataExtract)}.");
        await Task.CompletedTask;
    }
}

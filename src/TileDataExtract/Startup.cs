using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TileDataExtract;

internal class Startup
{
    private readonly ILogger<Startup> _logger;
    private readonly Settings _settings;

    public Startup(ILogger<Startup> logger, IOptions<Settings> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }

    public async Task StartAsync()
    {
        _logger.LogInformation($"Starting {nameof(TileDataExtract)}.");
        // TODO change to use output path
        await GeoJsonWriter.WriteAsync(_settings.Selections, _settings.ConnectionString, "")
            .ConfigureAwait(false);
        _logger.LogInformation($"Finished writing to.");
    }
}

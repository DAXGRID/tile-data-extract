using Microsoft.Extensions.Logging;

namespace TileDataExtract;

internal sealed class Startup
{
    private readonly ILogger<Startup> _logger;
    private readonly Settings _settings;

    public Startup(ILogger<Startup> logger, Settings settings)
    {
        _logger = logger;
        _settings = settings;
    }

    public async ValueTask StartAsync()
    {
        _logger.LogInformation($"Starting {nameof(TileDataExtract)}.");

        await GeoJsonWriter.WriteAsync(
            _settings.Selections,
            _settings.ConnectionString,
            _settings.OutputFilePath,
            (x) => _logger.LogInformation(x),
            (x) => _logger.LogError(x)
        ).ConfigureAwait(false);

        _logger.LogInformation($"Finished writing to {_settings.OutputFilePath}");
    }
}

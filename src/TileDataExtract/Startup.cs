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

    public async Task StartAsync()
    {
        _logger.LogInformation($"Starting {nameof(TileDataExtract)}.");

        const int maxRetries = 5;
        const int retryDelay = 1000;
        var retries = 0;
        var completed = false;

        while (!completed)
        {
            try
            {
                await GeoJsonWriter.WriteAsync(
                    _settings.Selections,
                    _settings.ConnectionString,
                    _settings.OutputFilePath,
                    (x) => _logger.LogInformation(x),
                    (x) => _logger.LogError(x)
                ).ConfigureAwait(false);

                completed = true;
            }
            catch (TimeoutException ex)
            {
                retries++;

                if (retries == maxRetries)
                {
                    throw new MaxRetriesReachedException(
                        "Max number of retries '{maxRetries}' reached.",
                        ex);
                }

                _logger.LogInformation("Received the following {Exeption}, retrying, {CurrentRetries} {MaxNumberOfRetries}.", ex, retries, maxRetries);

                await Task.Delay(retryDelay).ConfigureAwait(false);
            }
        }

        _logger.LogInformation($"Finished writing to {_settings.OutputFilePath}");
    }
}

using System.Text.Json;

namespace TileDataExtract;

internal static class GeoJsonWriter
{
    private readonly static JsonSerializerOptions _jsonSerializerOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public static async ValueTask WriteAsync(List<Selection> selections, string connectionString, string outputPath)
    {
        var id = 0;
        using var writer = new StreamWriter(outputPath);
        foreach (var selection in selections)
        {
            var reader = PostgresReader.ReadTableColumnsAsync(connectionString, selection.SqlQuery).ConfigureAwait(false);
            await foreach (var column in reader)
            {
                var geojsonLine = JsonSerializer.Serialize(GeoJsonFactory.Create(selection, column, id), _jsonSerializerOptions);
                await writer.WriteLineAsync(geojsonLine).ConfigureAwait(false);
                id++;
            }
        }
    }
}

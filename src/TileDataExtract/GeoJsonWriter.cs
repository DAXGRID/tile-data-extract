using System.Text;
using System.Text.Json;

namespace TileDataExtract;

internal static class GeoJsonWriter
{
    private readonly static JsonSerializerOptions _jsonSerializerOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public static async Task WriteAsync(
        List<Selection> selections, string connectionString, string outputPath)
    {
        // We remove the existing file since we append.
        // if (File.Exists(outputPath))
        //     File.Delete(outputPath);

        var id = 0;
        using StreamWriter writer = new(outputPath);
        foreach (var selection in selections)
        {
            var reader = PostgresReader.ReadTableColumnsAsync(connectionString, selection.SqlQuery)
                .ConfigureAwait(false);

            await foreach (var column in reader)
            {
                var geojson = JsonSerializer.Serialize(
                    GeoJsonFactory.Create(selection, column, id), _jsonSerializerOptions);
                await writer.WriteLineAsync(geojson).ConfigureAwait(false);
                id++;
            }
        }
    }
}

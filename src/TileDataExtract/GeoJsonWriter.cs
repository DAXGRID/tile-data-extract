using System.Text;
using System.Text.Json;

namespace TileDataExtract;

internal static class GeoJsonWriter
{
    public static async Task WriteAsync(
        List<Selection> selections, string connectionString, string outputPath)
    {
        foreach (var selection in selections)
        {
            var reader = PostgresReader.ReadTableColumnsAsync(
                connectionString, selection.SqlQuery).ConfigureAwait(false);

            var id = 0;
            await foreach (var column in reader)
            {
                var jsonNewline = JsonNewline(GeoJsonFactory.Create(selection, column, id));
                await File.AppendAllTextAsync(outputPath, jsonNewline, Encoding.UTF8)
                    .ConfigureAwait(false);
                id++;
            }
        }
    }

    private static string JsonNewline<T>(T x)
        => $"{JsonSerializer.Serialize(x)}\n";
}

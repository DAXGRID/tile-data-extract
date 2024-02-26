using NetTopologySuite.IO;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TileDataExtract;

internal static class GeoJsonWriter
{
    public static async ValueTask WriteAsync(
        List<Selection> selections,
        string connectionString,
        string outputPath,
        Action<string> processingInfoCallback,
        Action<string> processingErrorCallback)
    {
        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = { new GeometryConverter() }
        };

        var id = 0;
        using var writer = new StreamWriter(outputPath);
        var serializer = GeoJsonSerializer.Create(serializerSettings);
        foreach (var selection in selections)
        {
            processingInfoCallback($"Starting processing selection with SQL: {selection.SqlQuery}.");

            var reader = PostgresReader.ReadTableColumnsAsync(connectionString, selection.SqlQuery).ConfigureAwait(false);

            await foreach (var column in reader)
            {
                using (var stringWriter = new StringWriter())
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    try
                    {
                        serializer.Serialize(jsonWriter, GeoJsonFactory.Create(selection, column, id));
                        await writer.WriteLineAsync(stringWriter.ToString()).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        var failedColumn = $"Failed at {String.Join(", ", column.Select(x => $"{x.Key} : {x.Value?.ToString() ?? "NULL"}"))}.";
                        processingInfoCallback($"Failed at: {failedColumn}");
                        throw;
                    }
                }

                id++;
            }
        }
    }
}

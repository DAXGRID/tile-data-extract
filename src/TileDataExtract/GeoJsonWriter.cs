using NetTopologySuite.IO;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TileDataExtract;

internal static class GeoJsonWriter
{
    public static async ValueTask WriteAsync(List<Selection> selections, string connectionString, string outputPath)
    {
        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = { new GeometryConverter() }
        };

        using var writer = new StreamWriter(outputPath);
        foreach (var selection in selections)
        {
            var id = 0;
            var reader = PostgresReader.ReadTableColumnsAsync(connectionString, selection.SqlQuery).ConfigureAwait(false);

            await foreach (var column in reader)
            {
                var serializer = GeoJsonSerializer.Create(serializerSettings);
                using (var stringWriter = new StringWriter())
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    serializer.Serialize(jsonWriter, GeoJsonFactory.Create(selection, column, id));
                    await writer.WriteLineAsync(stringWriter.ToString()).ConfigureAwait(false);
                }

                id++;
            }
        }
    }
}

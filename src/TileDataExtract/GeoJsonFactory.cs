using System.Linq;
using System.Text.Json;

namespace TileDataExtract;

internal record Tippecanoe(int Minzoom, int Maxzoom);

internal record Geometry(string Type, double[] Coordinates);

internal record GeoJsonStructure(
    string Type,
    int Id,
    Geometry Geometry,
    Dictionary<string, object> Properties,
    Tippecanoe Tippecanoe);

internal static class GeoJsonFactory
{
    public static GeoJsonStructure Create(
        Selection selection,
        Dictionary<string, object> column)
    {
        var geometry = JsonSerializer
            .Deserialize<Geometry>((string)column[selection.GeometryFieldName]);

        var properties = column.Where(x => x.Key != selection.GeometryFieldName)
            .ToDictionary(x => x.Key, x => x.Value);

        var zoom = selection.CustomZooms

        return new GeoJsonStructure(
            selection.ObjectType,
            1,
            geometry,
            properties,
            new()
        );
    }
}

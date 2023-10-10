using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;

namespace TileDataExtract;

internal sealed record Tippecanoe(int Minzoom, int Maxzoom);

internal sealed record GeoJsonStructure(
    string Type,
    int Id,
    Geometry Geometry,
    Dictionary<string, object?> Properties,
    Tippecanoe Tippecanoe);

internal static class GeoJsonFactory
{
    public static GeoJsonStructure Create(
        Selection selection,
        Dictionary<string, object?> column,
        int id) =>
        new GeoJsonStructure(
            selection.ObjectType,
            id,
            CreateGeometry(selection, column),
            CreateProperties(selection, column),
            CreateTippecanoe(selection, column));

    private static Tippecanoe CreateTippecanoe(
        Selection selection, Dictionary<string, object?> column)
    {
        Tippecanoe tippecanoe;
        if (selection.CustomZoom is null)
        {
            tippecanoe = new(selection.DefaultZoom.MinZoom, selection.DefaultZoom.MaxZoom);
        }
        else
        {
            var customZoomValue = (string?)column[selection.CustomZoom.FieldName];
            Zoom? zoom;
            if (customZoomValue is not null &&
                selection.CustomZoom.ZoomMap.TryGetValue(customZoomValue, out zoom))
            {
                tippecanoe = new(zoom.MinZoom, zoom.MaxZoom);
            }
            else
            {
                tippecanoe = new(selection.DefaultZoom.MinZoom, selection.DefaultZoom.MaxZoom);
            }
        }

        return tippecanoe;
    }

    private static Dictionary<string, object?> CreateProperties(
        Selection selection, Dictionary<string, object?> column)
    {
        return column.Where(x => x.Key != selection.GeometryFieldName)
            .Concat(selection.ExtraProperties.ToDictionary(x => x.Key, y => (object?)y.Value))
            .ToDictionary(x => x.Key, x => x.Value);
    }

    private static Geometry CreateGeometry(
        Selection selection, Dictionary<string, object?> column)
    {
        var geoJsonGeometry = (string?)column[selection.GeometryFieldName] ?? "";

        var serializer = GeoJsonSerializer.Create();
        using (var stringReader = new StringReader(geoJsonGeometry))
        using (var jsonReader = new JsonTextReader(stringReader))
        {
            return serializer.Deserialize<NetTopologySuite.Geometries.Geometry>(jsonReader)
                ?? throw new ArgumentException($"Could not handle geometry '{geoJsonGeometry}'");
        }
    }
}

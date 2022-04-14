using System.Text.Json;
using System.Text.Json.Serialization;

namespace TileDataExtract;

internal record Tippecanoe(int Minzoom, int Maxzoom);

internal record Geometry
{
    [JsonPropertyName("type")]
    public string Type { get; init; }
    [JsonPropertyName("coordinates")]
    public dynamic Coordinates { get; init; }

    [JsonConstructor]
    public Geometry(string type, dynamic coordinates)
    {
        if (type == "Point")
            Coordinates = ((JsonElement)coordinates).Deserialize<double[]>()
                ?? throw new ArgumentException($"Couldn't deserialize {coordinates}.", nameof(coordinates));
        else if (type == "LineString")
            Coordinates = ((JsonElement)coordinates).Deserialize<double[][]>()
                ?? throw new ArgumentException($"Couldn't deserialize {coordinates}.", nameof(coordinates));
        else
            throw new ArgumentException($"Could not handle '{type}'.", nameof(type));

        Type = type;
    }
}

internal record GeoJsonStructure(
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
            .Concat(selection.ExtraProperties as Dictionary<string, object?>)
            .ToDictionary(x => x.Key, x => x.Value);
    }

    private static Geometry CreateGeometry(
        Selection selection, Dictionary<string, object?> column)
    {
        var geometry = (string?)column[selection.GeometryFieldName] ?? "";
        return JsonSerializer.Deserialize<Geometry>(geometry) ??
           throw new ArgumentException(
               $"Could not deserialize geometry with value: {geometry}.", nameof(geometry));
    }
}

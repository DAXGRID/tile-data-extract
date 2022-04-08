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
        throw new NotImplementedException();
    }
}

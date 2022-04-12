namespace TileDataExtract;

internal record CustomZoom(string FieldName, List<string> FieldValue, int MinZoom, int MaxZoom);

internal record Selection
{
    public string ObjectType { get; init; }
    public string GeometryFieldName { get; init; }
    public string SqlQuery { get; init; }
    public int DefaultMinZoom { get; init; }
    public int DefaultMaxZoom { get; init; }
    public List<CustomZoom> CustomZooms { get; init; }

    public Selection(
        string objectType,
        string geometryFieldName,
        string sqlQuery,
        int defaultMinZoom,
        int defaultMaxZoom,
        List<CustomZoom>? customZooms)
    {
        this.ObjectType = objectType;
        GeometryFieldName = geometryFieldName;
        SqlQuery = sqlQuery;
        DefaultMinZoom = defaultMinZoom;
        DefaultMaxZoom = defaultMaxZoom;
        CustomZooms = customZooms ?? new();
    }
}

internal record Settings(string ConnectionString, List<Selection> Selections);

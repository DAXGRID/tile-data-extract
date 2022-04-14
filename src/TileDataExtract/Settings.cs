namespace TileDataExtract;

internal record Zoom(int MinZoom, int MaxZoom);

internal record CustomZoom(
    string FieldName,
    Dictionary<string, Zoom> ZoomMap);

internal record Selection
{
    public string ObjectType { get; init; }
    public string GeometryFieldName { get; init; }
    public string SqlQuery { get; init; }
    public Zoom DefaultZoom { get; init; }
    public CustomZoom? CustomZoom { get; init; }
    public Dictionary<string, object> ExtraProperties { get; init; }

    public Selection(
        string objectType,
        string geometryFieldName,
        string sqlQuery,
        Zoom defaultZoom,
        CustomZoom? customZoom,
        Dictionary<string, object>? extraProperties)
    {
        ObjectType = objectType;
        GeometryFieldName = geometryFieldName;
        SqlQuery = sqlQuery;
        DefaultZoom = defaultZoom;
        CustomZoom = customZoom;
        ExtraProperties = extraProperties ?? new();
    }
}

internal record Settings(string OutputFilePath, string ConnectionString, List<Selection> Selections);

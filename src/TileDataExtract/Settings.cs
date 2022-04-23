using System.Text.Json.Serialization;

namespace TileDataExtract;

internal sealed record Zoom
{
    [JsonPropertyName("minZoom")]
    public int MinZoom { get; init; }
    [JsonPropertyName("maxZoom")]
    public int MaxZoom { get; init; }

    [JsonConstructor]
    public Zoom(int minZoom, int maxZoom)
    {
        MinZoom = minZoom;
        MaxZoom = maxZoom;
    }
}

internal sealed record CustomZoom
{
    [JsonPropertyName("fieldName")]
    public string FieldName { get; init; }
    [JsonPropertyName("zoomMap")]
    public Dictionary<string, Zoom> ZoomMap { get; init; }

    [JsonConstructor]
    public CustomZoom(string fieldName, Dictionary<string, Zoom> zoomMap)
    {
        FieldName = fieldName;
        ZoomMap = zoomMap;
    }
}

internal sealed record Selection
{
    [JsonPropertyName("objectType")]
    public string ObjectType { get; init; }
    [JsonPropertyName("geometryFieldName")]
    public string GeometryFieldName { get; init; }
    [JsonPropertyName("sqlQuery")]
    public string SqlQuery { get; init; }
    [JsonPropertyName("defaultZoom")]
    public Zoom DefaultZoom { get; init; }
    [JsonPropertyName("customZoom")]
    public CustomZoom? CustomZoom { get; init; }
    [JsonPropertyName("extraProperties")]
    public Dictionary<string, object> ExtraProperties { get; init; }

    [JsonConstructor]
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

internal sealed record Settings
{
    [JsonPropertyName("outputFilePath")]
    public string OutputFilePath { get; init; }
    [JsonPropertyName("connectionString")]
    public string ConnectionString { get; init; }
    [JsonPropertyName("selections")]
    public List<Selection> Selections { get; init; }

    [JsonConstructor]
    public Settings(string outputFilePath, string connectionString, List<Selection> selections)
    {
        OutputFilePath = outputFilePath;
        ConnectionString = connectionString;
        Selections = selections;
    }
}

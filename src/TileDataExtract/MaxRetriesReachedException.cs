namespace TileDataExtract;

public sealed class MaxRetriesReachedException : Exception
{
    public MaxRetriesReachedException() {}
    public MaxRetriesReachedException(string message): base(message) {}
    public MaxRetriesReachedException(string message, Exception innerException): base(message, innerException) {}
}

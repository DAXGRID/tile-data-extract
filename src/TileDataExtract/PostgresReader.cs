using Npgsql;

namespace TileDataExtract;

internal class PostgresReader
{
    public static async IAsyncEnumerable<string> ReadTable(
        string connectionString, string query)
    {
        using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync().ConfigureAwait(false);

        using var cmd = new NpgsqlCommand(query, conn);

        var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            yield return reader.GetGuid(0).ToString();
        }
    }
}

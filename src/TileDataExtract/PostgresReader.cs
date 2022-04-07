using Npgsql;

namespace TileDataExtract;

internal class PostgresReader
{
    public static async IAsyncEnumerable<string> ReadTableColumnsAsync(
        string connectionString, string query)
    {
        using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync().ConfigureAwait(false);

        using var cmd = new NpgsqlCommand(query, conn);
        var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            var column = new Dictionary<string, object>();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (!reader.GetDataTypeName(i)
                    .Contains("geometry", StringComparison.OrdinalIgnoreCase))
                {
                    column.Add(reader.GetName(i), reader.GetValue(i));
                }
                else
                {
                    // Do nothing right now
                }
            }

            yield return reader.GetGuid(0).ToString();
        }
    }
}

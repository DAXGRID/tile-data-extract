using Npgsql;

namespace TileDataExtract;

internal sealed class PostgresReader
{
    public static async IAsyncEnumerable<Dictionary<string, object?>>
        ReadTableColumnsAsync(string connectionString, string query)
    {
        using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync().ConfigureAwait(false);

        using var cmd = new NpgsqlCommand(query, conn)
        {
            // Gives the request two minutes to complete.
            // The result set can be rather large, so sometimes the default 30 seconds is not enough.
            CommandTimeout = 120
        };

        var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            var column = new Dictionary<string, object?>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (!reader.GetDataTypeName(i)
                    .Contains("geometry", StringComparison.OrdinalIgnoreCase))
                {
                    column.Add(reader.GetName(i), reader.IsDBNull(i) ? null : reader.GetValue(i));
                }
            }

            yield return column;
        }
    }
}

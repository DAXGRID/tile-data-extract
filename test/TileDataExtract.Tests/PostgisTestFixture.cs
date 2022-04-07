using Npgsql;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace TileDataExtract.Tests;

public class PostgisTestFixture : IAsyncLifetime
{
    public const string ConnectionString = "Host=localhost;Port=5432;Username=docker;Password=docker;Database=postgres";

    public async Task InitializeAsync()
    {
        await SetupDB();
    }

    public async Task DisposeAsync()
    {
        var sql = "DROP SCHEMA route_network CASCADE";
        using var conn = new NpgsqlConnection(ConnectionString);
        using var cmd = new NpgsqlCommand(sql, conn);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private async Task SetupDB()
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        var sql = File.ReadAllText(GetFilePath("Scripts/setup_db.sql"));
        using var cmd = new NpgsqlCommand(sql, conn);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private static string GetFilePath(string filePath)
    {
        var absolutePath = Path.IsPathRooted(filePath)
            ? filePath
            : Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);

        return File.Exists(absolutePath)
            ? absolutePath
            : throw new ArgumentException(
                $"Could not find file at path: {absolutePath}");
    }
}

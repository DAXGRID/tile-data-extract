using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace TileDataExtract.Tests;

[Trait("Category", "Integration")]
public class PostgresReaderTests : IClassFixture<PostgisTestFixture>
{
    [Theory]
    [JsonFileData("Data/route_node.json")]
    public async Task Read_table(string json)
    {
        var expected = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);

        var sql = "select * from route_network.route_node";
        var column = await PostgresReader.ReadTableColumnsAsync(
                         PostgisTestFixture.ConnectionString, sql).ToListAsync();

        column.Should().BeEquivalentTo(expected);
    }
}

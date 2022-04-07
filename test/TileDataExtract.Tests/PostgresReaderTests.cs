using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TileDataExtract.Tests;

[Trait("Category", "Integration")]
public class PostgresReaderTests : IClassFixture<PostgisTestFixture>
{
    [Fact]
    public async Task Read_table()
    {
        var sql = "select * from route_network.route_node";
        var count = (await PostgresReader.ReadTable(PostgisTestFixture.ConnectionString, sql).ToListAsync()).Count;
        count.Should().Be(10);
    }
}

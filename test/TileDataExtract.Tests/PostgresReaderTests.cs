using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace TileDataExtract.Tests;

[Trait("Category", "Integration")]
[Collection("Postgis collection")]
public class PostgresReaderTests
{
    [Theory]
    [JsonFileData("Data/route_node.json")]
    public async Task Read_table(string expectedJson)
    {
        var expected = JToken.Parse(expectedJson);

        var sql =
              @"select
                  mrid,
                  ST_AsGeoJSON(ST_Transform(coord,4326)) as coord,
                  routenode_kind,
                  routenode_function,
                  naming_name,
                  mapping_method,
                  lifecycle_deployment_state
                from
                  route_network.route_node
                where
                  coord is not null and
                  marked_to_be_deleted = false";

        var columns = await PostgresReader.ReadTableColumnsAsync(
                         PostgisTestFixture.ConnectionString, sql).ToListAsync();

        JToken.Parse(JsonSerializer.Serialize(columns)).Should().BeEquivalentTo(expected);
    }
}

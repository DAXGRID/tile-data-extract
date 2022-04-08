using FluentAssertions;
using Newtonsoft.Json.Linq;
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
    public async Task Read_table(string expected)
    {
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

        JToken.Parse(JsonSerializer.Serialize(columns))
            .Should()
            .BeEquivalentTo(JToken.Parse(expected));
    }
}

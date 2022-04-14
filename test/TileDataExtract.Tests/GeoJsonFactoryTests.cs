using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TileDataExtract.Tests;

public class GeoJsonFactoryTests
{
    [Fact]
    public void Create()
    {
        var settings = CreateSettings();
        var id = 2;
        var tippecanoe = new Tippecanoe(17, 17);
        var geometry = new Geometry("Point", new double[] { 9.840274737, 55.848383545 });
        var properties = new Dictionary<string, object?>
        {
            { "mrid", "06e660e2-8a6b-4f1b-bc7f-85f1aea8ca5f" },
            { "objecttype", "route_node" },
            { "routenode_kind", null },
            { "routenode_function", null },
            { "naming_name", null },
            { "mapping_method", null },
            { "lifecycle_deployment_state", null }
        };

        var expected = new GeoJsonStructure("Feature", id, geometry, properties, tippecanoe);
        var selection = settings.Selections.First();

        var column = new Dictionary<string, object?>
        {
            { "mrid", "06e660e2-8a6b-4f1b-bc7f-85f1aea8ca5f" },
            { "coord", "{\"type\":\"Point\",\"coordinates\":[9.840274737,55.848383545]}" },
            { "routenode_kind", null },
            { "routenode_function", null },
            { "naming_name", null },
            { "mapping_method", null },
            { "lifecycle_deployment_state", null }
        };

        var result = GeoJsonFactory.Create(selection, column, id);

        result.Should().BeEquivalentTo(expected);
    }

    private static Settings CreateSettings()
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

        var connString = "Host=localhost;Port=5432;Username=docker;Password=docker;Database=postgres";

        var selections = new List<Selection>
        {
            new Selection(
                "Feature",
                "coord",
                sql,
                new (17, 17),
                new CustomZoom(
                    "routenode_kind",
                    new Dictionary<string, Zoom>
                    {
                        {"CentralOfficeBig", new (5, 22)},
                        {"CentralOfficeMedium", new (5, 22)},
                        {"CentralOfficeSmall", new (5, 22)},
                        {"CabinetBig", new (12, 22)}
                    }),
                new()
                {
                    {"objecttype", "route_node"}
                }
            )
        };

        return new Settings(connString, selections);
    }
}

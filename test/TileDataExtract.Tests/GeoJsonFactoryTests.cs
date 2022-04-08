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
        var tippecanoe = new Tippecanoe(17, 17);
        var geometry = new Geometry("Point", new double[] { 9.840274737, 55.848383545 });
        var properties = new Dictionary<string, object>
        {
            { "objecttype", "route_node" },
            { "mrid", "0415770b-27e6-421c-b6bb-7a40dc2165f6" }
        };

        var expected = new GeoJsonStructure("Feature", 1, geometry, properties, tippecanoe);
        var selection = settings.Selections.First();

        var column = new Dictionary<string, object>
        {
            { "mrid", "0415770b-27e6-421c-b6bb-7a40dc2165f6" },
        };

        var result = GeoJsonFactory.Create(selection, column);

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
                "route_node",
                "coord",
                sql,
                17,
                17,
                new List<CustomZoom>
                {
                    new CustomZoom(
                        "routenode_kind",
                        new List<string>
                        {
                            "CentralOfficeBig",
                            "CentralOfficeMedium",
                            "CentralOfficeSmall"
                        },
                        5,
                        22),
                    new CustomZoom(
                        "routenode_kind",
                        new List<string> {"CabinetBig"},
                        12,
                        22),
                }
            )
        };

        return new Settings(connString, selections);
    }
}

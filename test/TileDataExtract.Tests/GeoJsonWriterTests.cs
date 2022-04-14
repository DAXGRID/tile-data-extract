using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TileDataExtract.Tests;
using Xunit;

namespace TileDataExtract;

[Trait("Category", "Integration")]
[Collection("Postgis collection")]
public class GeoJsonWriterTests
{
    [Theory]
    [JsonFileData("Data/route_network.geojson")]
    public async Task Read_from_postgres_and_write_geojson_to_disk(string expected)
    {
        var settings = CreateSettings();
        var outputPath = $"{Path.GetTempPath()}/{Guid.NewGuid().ToString()}.geojson";
        await GeoJsonWriter.WriteAsync(settings.Selections, settings.ConnectionString, outputPath);

        var result = await File.ReadAllTextAsync(outputPath);

        result.Should().Be(expected);
    }

    private static Settings CreateSettings()
    {
        var selectionRouteNodes = new Selection(
            "Feature",
            "coord",
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
                  marked_to_be_deleted = false",
            new(17, 17),
            new CustomZoom(
                "routenode_kind",
                new Dictionary<string, Zoom>
                {
                    {"CentralOfficeBig", new(5, 22)},
                    {"CentralOfficeMedium", new(5, 22)},
                    {"CentralOfficeSmall", new(5, 22)},
                    {"CabinetBig", new(12, 22)}
                }),
            new()
            {
                {"objecttype", "route_node"}
            }
        );

        var selectionRouteSegments = new Selection(
            "Feature",
            "coord",
            @"select
                mrid,
                ST_AsGeoJSON(ST_Transform(coord, 4326)) as coord,
                routesegment_kind,
                mapping_method,
                lifecycle_deployment_state
              from
                route_network.route_segment
              where
                coord is not null and
                marked_to_be_deleted = false",
            new(12, 22),
            null,
            new()
            {
                {"objecttype", "route_segment"}
            }
        );

        var selections = new List<Selection>
        {
            selectionRouteNodes,
            selectionRouteSegments
        };

        return new Settings(PostgisTestFixture.ConnectionString, selections);
    }
}

using Xunit;

namespace TileDataExtract.Tests;

[CollectionDefinition("Postgis collection")]
public class PostgisCollection : ICollectionFixture<PostgisTestFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

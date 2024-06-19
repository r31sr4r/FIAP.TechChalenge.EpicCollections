using FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.GetCollection;

[CollectionDefinition(nameof(GetCollectionApiTestFixture))]
public class GetCollectionApiTestFixtureCollection : ICollectionFixture<GetCollectionApiTestFixture>
{ }

public class GetCollectionApiTestFixture : CollectionBaseFixture
{
}

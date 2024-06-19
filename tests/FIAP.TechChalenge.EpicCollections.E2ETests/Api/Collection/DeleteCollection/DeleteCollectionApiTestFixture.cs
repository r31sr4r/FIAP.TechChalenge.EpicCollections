using FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.DeleteCollection;

[CollectionDefinition(nameof(DeleteCollectionApiTestFixture))]
public class DeleteCollectionApiTestFixtureCollection
    : ICollectionFixture<DeleteCollectionApiTestFixture>
{ }

public class DeleteCollectionApiTestFixture
    : CollectionBaseFixture
{
}

using FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.DeleteCollection;
[CollectionDefinition(nameof(DeleteCollectionTestFixture))]
public class DeleteCollectionTestFixtureCollection
    : ICollectionFixture<DeleteCollectionTestFixture>
{ }

public class DeleteCollectionTestFixture
    : CollectionUseCasesBaseFixture
{ }

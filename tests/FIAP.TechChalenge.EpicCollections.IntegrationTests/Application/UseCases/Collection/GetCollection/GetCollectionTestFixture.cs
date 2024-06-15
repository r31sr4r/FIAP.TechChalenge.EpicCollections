using FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.GetCollection;

[CollectionDefinition(nameof(GetCollectionTestFixture))]
public class GetCollectionTestFixtureCollection
    : ICollectionFixture<GetCollectionTestFixture>
{ }

public class GetCollectionTestFixture
    : CollectionUseCasesBaseFixture
{ }

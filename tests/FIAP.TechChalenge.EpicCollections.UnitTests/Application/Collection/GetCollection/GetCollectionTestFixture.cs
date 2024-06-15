using FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.Common;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.GetCollection;
[CollectionDefinition(nameof(GetCollectionTestFixture))]
public class GetCollectionTestFixtureCollection :
    ICollectionFixture<GetCollectionTestFixture>
{ }

public class GetCollectionTestFixture
    : CollectionUseCasesBaseFixture
{ }

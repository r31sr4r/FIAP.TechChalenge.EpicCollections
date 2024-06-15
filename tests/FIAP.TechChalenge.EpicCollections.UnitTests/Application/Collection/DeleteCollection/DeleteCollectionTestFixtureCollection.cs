using FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.Common;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.DeleteCollection;
[CollectionDefinition(nameof(DeleteCollectionTestFixture))]
public class DeleteCollectionTestFixtureCollection
    : ICollectionFixture<DeleteCollectionTestFixture>
{ }

public class DeleteCollectionTestFixture
    : CollectionUseCasesBaseFixture
{ }

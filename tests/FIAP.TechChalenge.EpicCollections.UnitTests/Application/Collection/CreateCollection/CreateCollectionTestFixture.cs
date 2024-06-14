using Bogus;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.CreateCollection;
using FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.Common;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.CreateCollection;

[CollectionDefinition(nameof(CreateCollectionTestFixture))]
public class CreateCollectionTestFixtureCollection : ICollectionFixture<CreateCollectionTestFixture> { }

public class CreateCollectionTestFixture : CollectionUseCasesBaseFixture
{
    public CreateCollectionInput GetInput(Guid userId)
    {
        var collection = GetValidCollection(userId);
        return new CreateCollectionInput(
            collection.UserId,
            collection.Name,
            collection.Description,
            collection.Category
        );
    }

    public CreateCollectionInput GetInputWithInvalidName(Guid userId)
    {
        var collection = GetInput(userId);
        collection.Name = string.Empty;
        return collection;
    }

    public CreateCollectionInput GetInputWithShortName(Guid userId)
    {
        var collection = GetInput(userId);
        collection.Name = "Ab";
        return collection;
    }

    public CreateCollectionInput GetInputWithLongName(Guid userId)
    {
        var collection = GetInput(userId);
        collection.Name = new string('A', 256);
        return collection;
    }

    public CreateCollectionInput GetInputWithEmptyDescription(Guid userId)
    {
        var collection = GetInput(userId);
        collection.Description = string.Empty;
        return collection;
    }
}


using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.CreateCollection;
using FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.CreateCollection;

[CollectionDefinition(nameof(CreateCollectionTestFixture))]
public class CreateCollectionTestFixtureCollection : ICollectionFixture<CreateCollectionTestFixture>
{ }

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

    public CreateCollectionInput GetInputWithInvalidName()
    {
        var collection = GetInput(Guid.NewGuid());
        collection.Name = string.Empty;
        return collection;
    }

    public CreateCollectionInput GetInputWithShortName()
    {
        var collection = GetInput(Guid.NewGuid());
        collection.Name = "Ab";
        return collection;
    }

    public CreateCollectionInput GetInputWithLongName()
    {
        var collection = GetInput(Guid.NewGuid());
        collection.Name = new string('A', 256);
        return collection;
    }

    public CreateCollectionInput GetInputWithEmptyDescription()
    {
        var collection = GetInput(Guid.NewGuid());
        collection.Description = string.Empty;
        return collection;
    }
}

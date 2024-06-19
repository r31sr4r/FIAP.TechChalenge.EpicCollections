using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.CreateCollection;
using FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.CreateCollection;

[CollectionDefinition(nameof(CreateCollectionApiTestFixture))]
public class CreateCollectionApiTestFixtureCollection
: ICollectionFixture<CreateCollectionApiTestFixture>
{ }

public class CreateCollectionApiTestFixture
    : CollectionBaseFixture
{
    public CreateCollectionInput GetInput()
    {
        var collection = GetValidCollection(AuthenticatedUser.Id);
        return new CreateCollectionInput(
            AuthenticatedUser.Id,
            collection.Name,
            collection.Description,
            collection.Category
        );
    }
}

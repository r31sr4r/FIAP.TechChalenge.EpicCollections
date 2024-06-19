using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollection;
using FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.UpdateCollection;

[CollectionDefinition(nameof(UpdateCollectionApiTestFixture))]
public class UpdateCollectionApiTestFixtureCollection : ICollectionFixture<UpdateCollectionApiTestFixture>
{ }

public class UpdateCollectionApiTestFixture : CollectionBaseFixture
{
    public UpdateCollectionInput GetInput(Guid userId)
    {
        var collection = GetValidCollection(userId);
        return new UpdateCollectionInput(
            collection.Id,
            collection.Name,
            collection.Description,
            collection.Category
        );
    }
}
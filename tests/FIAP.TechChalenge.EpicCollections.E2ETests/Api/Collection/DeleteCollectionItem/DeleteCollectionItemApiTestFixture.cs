using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollectionItem;
using FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.DeleteCollectionItem;

[CollectionDefinition(nameof(DeleteCollectionItemApiTestFixture))]
public class DeleteCollectionItemApiTestFixtureCollection
    : ICollectionFixture<DeleteCollectionItemApiTestFixture>
{ }
public class DeleteCollectionItemApiTestFixture
    : CollectionBaseFixture
{
    public DeleteCollectionItemInput GetInput(Guid collectionId, Guid itemId)
    {
        return new DeleteCollectionItemInput(
            collectionId,
            itemId
        );
    }

    public async Task<(Guid collectionId, Guid itemId)> CreateCollectionWithItem()
    {
        var collection = GetValidCollection(AuthenticatedUser.Id);
        await Persistence.Insert(collection);

        var item = GetValidCollectionItem(collection.Id);
        await Persistence.AddItemToCollection(item);

        return (collection.Id, item.Id);
    }

}

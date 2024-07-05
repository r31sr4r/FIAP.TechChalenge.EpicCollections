using FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollectionItem;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.GetCollectionItem;

[CollectionDefinition(nameof(GetCollectionItemApiTestFixture))]
public class GetCollectionItemApiTestFixtureCollection : ICollectionFixture<GetCollectionItemApiTestFixture> { }

public class GetCollectionItemApiTestFixture : CollectionBaseFixture
{
    public GetCollectionItemInput GetInput(Guid collectionId, Guid itemId)
    {
        return new GetCollectionItemInput(
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

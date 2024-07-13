using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.ListCollectionItems;

[CollectionDefinition(nameof(ListCollectionItemsApiTestFixture))]
public class ListCollectionItemsApiTestFixtureCollection : ICollectionFixture<ListCollectionItemsApiTestFixture>
{ }

public class ListCollectionItemsApiTestFixture : CollectionBaseFixture
{
    public List<DomainEntity.Collection.CollectionItem> GetCollectionItemsList(Guid collectionId, int length = 10)
    {
        var list = new List<DomainEntity.Collection.CollectionItem>();
        for (int i = 0; i < length; i++)
        {
            list.Add(new DomainEntity.Collection.CollectionItem(
                collectionId,
                GetValidCollectionName(),
                GetValidDescription(),
                DateTime.Now,
                Faker.Random.Decimal(1, 1000),
                Faker.Internet.Url()
            ));
        }
        return list;
    }

    public DomainEntity.Collection.Collection GetCollectionWithItems(Guid userId)
    {
        var collection = new DomainEntity.Collection.Collection(
            userId,
            GetValidCollectionName(),
            GetValidDescription(),
            GetValidCategory()
        );

        var items = GetCollectionItemsList(collection.Id, 10);
        foreach (var item in items)
        {
            collection.AddItem(item);
        }

        return collection;
    }
}

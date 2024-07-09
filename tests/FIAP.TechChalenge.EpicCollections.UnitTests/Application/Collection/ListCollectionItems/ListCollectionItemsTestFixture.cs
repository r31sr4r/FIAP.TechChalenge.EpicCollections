using Bogus;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollectionItems;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.Common;
using Xunit;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.ListCollectionItems;

[CollectionDefinition(nameof(ListCollectionItemsTestFixture))]
public class ListCollectionItemsTestFixtureCollection : ICollectionFixture<ListCollectionItemsTestFixture> { }

public class ListCollectionItemsTestFixture : CollectionUseCasesBaseFixture
{
    public List<DomainEntity.Collection.CollectionItem> GetCollectionItemsList(Guid collectionId, int length = 10)
    {
        var list = new List<DomainEntity.Collection.CollectionItem>();
        for (int i = 0; i < length; i++)
        {
            list.Add(GetValidCollectionItem(collectionId));
        }
        return list;
    }

    public DomainEntity.Collection.Collection GetCollectionWithItems(Guid userId, int length = 10)
    {
        var collection = GetValidCollection(userId);
        var items = GetCollectionItemsList(collection.Id, length);
        foreach (var item in items)
        {
            collection.AddItem(item);
        }
        return collection;
    }

    public DomainEntity.Collection.CollectionItem GetValidCollectionItem(Guid collectionId)
        => new(
            collectionId,
            GetValidName(),
            GetValidDescription(),
            DateTime.Now,
            Faker.Random.Decimal(1, 1000),
            Faker.Internet.Url()
        );

    public ListCollectionItemsInput GetInput(Guid collectionId)
    {
        var random = new Random();
        return new ListCollectionItemsInput(
            collectionId: collectionId,
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
        );
    }

    public SearchOutput<DomainEntity.Collection.Collection> GetSearchOutput(List<DomainEntity.Collection.Collection> items)
    {
        return new SearchOutput<DomainEntity.Collection.Collection>(
            currentPage: 1,
            perPage: items.Count,
            total: items.Count,
            items: items
        );
    }
}

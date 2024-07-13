using Bogus;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollectionItems;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.Common;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.ListCollectionItems;

[CollectionDefinition(nameof(ListCollectionItemsTestFixture))]
public class ListCollectionItemsTestFixtureCollection : ICollectionFixture<ListCollectionItemsTestFixture> { }

public class ListCollectionItemsTestFixture : CollectionUseCasesBaseFixture
{
    public List<DomainEntity.Collection.Collection> GetCollectionWithItemsList(int length = 10)
    {
        var userId = Guid.NewGuid();
        var list = new List<DomainEntity.Collection.Collection>();
        for (int i = 0; i < length; i++)
        {
            list.Add(GetCollectionWithItems(userId));
        }
        return list;
    }

    public List<DomainEntity.Collection.CollectionItem> SortCollectionItems(
        List<DomainEntity.Collection.CollectionItem> items,
        string orderBy,
        SearchOrder order
    )
    {
        var orderedEnumerable = (orderBy, order) switch
        {
            ("name", SearchOrder.Asc) => items.OrderBy(x => x.Name).ToList(),
            ("name", SearchOrder.Desc) => items.OrderByDescending(x => x.Name).ToList(),
            ("acquisitionDate", SearchOrder.Asc) => items.OrderBy(x => x.AcquisitionDate).ToList(),
            ("acquisitionDate", SearchOrder.Desc) => items.OrderByDescending(x => x.AcquisitionDate).ToList(),
            _ => items.OrderBy(x => x.Name).ToList(),
        };

        return orderedEnumerable.ToList();
    }

    public DomainEntity.Collection.Collection GetCollectionWithItems(Guid? userId = null, int numberOfItems = 10)
    {
        var collection = GetValidCollection(userId ?? Guid.NewGuid());
        for (int i = 0; i < numberOfItems; i++)
        {
            collection.AddItem(GetValidCollectionItem(collection.Id));
        }
        return collection;
    }

    public List<DomainEntity.Collection.CollectionItem> GetCollectionItemsList(Guid collectionId, int length = 10)
    {
        var list = new List<DomainEntity.Collection.CollectionItem>();
        for (int i = 0; i < length; i++)
        {
            list.Add(GetValidCollectionItem(collectionId));
        }
        return list;
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

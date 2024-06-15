using Bogus;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollections;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.Common;
using Xunit;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.ListCollections;

[CollectionDefinition(nameof(ListCollectionsTestFixture))]
public class ListCollectionsTestFixtureCollection
    : ICollectionFixture<ListCollectionsTestFixture>
{ }

public class ListCollectionsTestFixture
    : CollectionUseCasesBaseFixture
{
    public List<DomainEntity.Collection> GetCollectionsList(int length = 10)
    {
        var list = new List<DomainEntity.Collection>();
        var userId = Guid.NewGuid();
        for (int i = 0; i < length; i++)
        {
            list.Add(GetValidCollection(userId));
        }
        return list;
    }

    public ListCollectionsInput GetInput()
    {
        var random = new Random();
        return new ListCollectionsInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
        );
    }

    public SearchOutput<DomainEntity.Collection> GetSearchOutput(List<DomainEntity.Collection> items)
    {
        return new SearchOutput<DomainEntity.Collection>(
            currentPage: 1,
            perPage: items.Count,
            total: items.Count,
            items: items
        );
    }
}

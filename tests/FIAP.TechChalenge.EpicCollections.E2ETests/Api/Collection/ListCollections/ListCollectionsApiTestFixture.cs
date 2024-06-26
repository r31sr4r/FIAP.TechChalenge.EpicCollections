using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.ListCollections;

[CollectionDefinition(nameof(ListCollectionsApiTestFixture))]
public class ListCollectionsApiTestFixtureCollection : ICollectionFixture<ListCollectionsApiTestFixture>
{ }

public class ListCollectionsApiTestFixture : CollectionBaseFixture
{
    public List<DomainEntity.Collection.Collection> GetExampleCollectionsListWithNames(Guid userId, List<string> names)
        => names.Select(name => new DomainEntity.Collection.Collection(
            userId,
            name,
            GetValidDescription(),
            GetValidCategory()
        )).ToList();

    public List<DomainEntity.Collection.Collection> SortList(
        List<DomainEntity.Collection.Collection> collectionsList,
        string orderBy,
        SearchOrder order
    )
    {
        var listClone = new List<DomainEntity.Collection.Collection>(collectionsList);
        var orderedEnumerable = (orderBy, order) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name),
            ("createdAt", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdAt", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name),
        };

        return orderedEnumerable
            .ThenBy(x => x.CreatedAt)
            .ToList();
    }
}

using Bogus;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollections;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.Common;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.ListCollections;

[CollectionDefinition(nameof(ListCollectionsTestFixture))]
public class ListCollectionsTestFixtureCollection
    : ICollectionFixture<ListCollectionsTestFixture>
{ }

public class ListCollectionsTestFixture
    : CollectionUseCasesBaseFixture
{
    public List<DomainEntity.Collection> GetExampleCollectionsListWithNames(List<string> names)
    {
        var userId = Guid.NewGuid();
        return names.Select(name => new DomainEntity.Collection(
            userId,
            name,
            GetValidDescription(),
            GetValidCategory()
        )).ToList();
    }

    public List<DomainEntity.Collection> SortList(
        List<DomainEntity.Collection> collectionsList,
        string orderBy,
        SearchOrder order
    )
    {
        var listClone = new List<DomainEntity.Collection>(collectionsList);
        var orderedEnumerable = (orderBy, order) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name).ToList(),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name).ToList(),
            ("createdAt", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt).ToList(),
            ("createdAt", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt).ToList(),
            _ => listClone.OrderBy(x => x.Name).ToList(),
        };

        return orderedEnumerable.ToList();
    }
}

using Bogus.Extensions.Brazil;
using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;
using FIAP.TechChalenge.EpicCollections.Domain.Entity;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.IntegrationTests.Base;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Infra.Data.EF.Repositories.CollectionRepository
{
    [CollectionDefinition(nameof(CollectionRepositoryTestFixture))]
    public class CollectionRepositoryTestFixtureCollection
        : ICollectionFixture<CollectionRepositoryTestFixture>
    { }

    public class CollectionRepositoryTestFixture
        : BaseFixture
    {

        public string GetValidCollectionName()
            => Faker.Commerce.ProductName();

        public string GetValidDescription()
            => Faker.Lorem.Paragraph();

        public Category GetRandomCategory()
            => Faker.PickRandom<Category>();

        public DateTime GetRandomDate()
            => Faker.Date.Past();

        public Collection GetExampleCollection(Guid userId)
            => new(
                userId,
                GetValidCollectionName(),
                GetValidDescription(),
                GetRandomCategory()
            );

        public List<Collection> GetExampleCollectionList(Guid userId, int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetExampleCollection(userId)).ToList();

        public List<Collection> SortList(
            List<Collection> collectionList,
            string orderBy,
            SearchOrder order
        )
        {
            var listClone = new List<Collection>(collectionList);
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

        public List<Collection> GetExampleCollectionListWithNames(Guid userId, List<string> names)
        {
            return names.Select(name => new Collection(
                userId,
                name,
                GetValidDescription(),
                GetRandomCategory()
            )).ToList();
        }


    }
}

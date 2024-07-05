using FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection;
using FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.Common;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.GetCollectionItem
{
    [CollectionDefinition(nameof(GetCollectionItemTestFixture))]
    public class GetCollectionItemTestFixtureCollection : ICollectionFixture<GetCollectionItemTestFixture> { }

    public class GetCollectionItemTestFixture : CollectionUseCasesBaseFixture
    {
        public CollectionItem GetValidCollectionItem(Guid collectionId)
        {
            return new CollectionItem(
                collectionId,
                GetValidName(),
                GetValidDescription(),
                GetValidAcquisitionDate(),
                GetValidValue(),
                Faker.Internet.Url()
            );
        }
    }
}

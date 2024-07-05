using FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.Common;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.GetCollectionItem;

[CollectionDefinition(nameof(GetCollectionItemTestFixture))]
public class GetCollectionItemTestFixtureCollection : ICollectionFixture<GetCollectionItemTestFixture> { }

public class GetCollectionItemTestFixture : CollectionUseCasesBaseFixture
{
    public DomainEntity.Collection.CollectionItem GetValidCollectionItem(Guid collectionId)
    => new(
        collectionId,
        GetValidName(),
        GetValidDescription(),
        DateTime.Now,
        Faker.Random.Decimal(1, 1000),
        Faker.Internet.Url()
    );
}

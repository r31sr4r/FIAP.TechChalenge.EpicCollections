using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollectionItem;
using FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.Common;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.DeleteCollectionItem;

[CollectionDefinition(nameof(DeleteCollectionItemTestFixture))]
public class DeleteCollectionItemTestFixtureCollection : ICollectionFixture<DeleteCollectionItemTestFixture>
{ }

public class DeleteCollectionItemTestFixture : CollectionUseCasesBaseFixture
{
    public DeleteCollectionItemInput GetValidInput(Guid collectionId, Guid itemId)
    {
        return new DeleteCollectionItemInput(collectionId, itemId);
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
}

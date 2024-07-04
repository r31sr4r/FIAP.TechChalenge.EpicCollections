using Bogus;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollectionItem;
using FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.Common;
using Xunit;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.DeleteCollectionItem;

[CollectionDefinition(nameof(DeleteCollectionItemTestFixture))]
public class DeleteCollectionItemTestFixtureCollection : ICollectionFixture<DeleteCollectionItemTestFixture> { }

public class DeleteCollectionItemTestFixture : CollectionUseCasesBaseFixture
{
    public DeleteCollectionItemInput GetValidInput(Guid collectionId, Guid itemId)
    {
        return new DeleteCollectionItemInput(collectionId, itemId);
    }

    public DomainEntity.CollectionItem GetValidCollectionItem(Guid collectionId)
    => new(
        collectionId,
        GetValidName(),
        GetValidDescription(),
        DateTime.Now,
        Faker.Random.Decimal(1, 1000),
        Faker.Internet.Url()
    );

}

using Bogus;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollectionItem;
using FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection;
using FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.Common;
using Xunit;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.UpdateCollectionItem;
[CollectionDefinition(nameof(UpdateCollectionItemTestFixture))]
public class UpdateCollectionItemTestFixtureCollection
    : ICollectionFixture<UpdateCollectionItemTestFixture>
{ }

public class UpdateCollectionItemTestFixture
    : CollectionUseCasesBaseFixture
{
    public UpdateCollectionItemInput GetValidInput(Guid? collectionId = null, Guid? itemId = null)
        => new UpdateCollectionItemInput(
            collectionId ?? Guid.NewGuid(),
            itemId ?? Guid.NewGuid(),
            GetValidName(),
            GetValidDescription(),
            DateTime.Now,
            Faker.Random.Decimal(1, 1000),
            Faker.Internet.Url()
        );

    public DomainEntity.Collection.Collection GetValidCollectionWithItems()
    {
        var collection = new DomainEntity.Collection.Collection(
            Guid.NewGuid(),
            GetValidName(),
            GetValidDescription(),
            GetValidCategory()
        );

        for (int i = 0; i < 10; i++)
        {
            collection.AddItem(new CollectionItem(
                collection.Id,
                GetValidName(),
                GetValidDescription(),
                DateTime.Now,
                Faker.Random.Decimal(1, 1000),
                Faker.Internet.Url()
            ));
        }

        return collection;
    }
}

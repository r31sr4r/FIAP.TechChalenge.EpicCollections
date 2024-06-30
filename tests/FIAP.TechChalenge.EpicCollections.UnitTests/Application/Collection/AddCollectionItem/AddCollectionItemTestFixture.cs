using Bogus;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;
using FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.Common;
using Xunit;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.AddCollectionItem;

[CollectionDefinition(nameof(AddCollectionItemTestFixture))]
public class AddCollectionItemTestFixtureCollection : ICollectionFixture<AddCollectionItemTestFixture> { }

public class AddCollectionItemTestFixture : CollectionUseCasesBaseFixture
{
    public AddCollectionItemInput GetValidInput(Guid collectionId)
    {
        var item = GetValidCollectionItem(collectionId);
        return new AddCollectionItemInput(
            item.CollectionId,
            item.Name,
            item.Description,
            item.AcquisitionDate,
            item.Value,
            item.PhotoUrl
        );
    }

    public AddCollectionItemInput GetInputWithInvalidName(Guid collectionId)
    {
        var item = GetValidCollectionItem(collectionId);
        return new AddCollectionItemInput(
            item.CollectionId,
            string.Empty,
            item.Description,
            item.AcquisitionDate,
            item.Value,
            item.PhotoUrl
        );
    }

    public AddCollectionItemInput GetInputWithShortName(Guid collectionId)
    {
        var item = GetValidCollectionItem(collectionId);
        return new AddCollectionItemInput(
            item.CollectionId,
            "Ab",
            item.Description,
            item.AcquisitionDate,
            item.Value,
            item.PhotoUrl
        );
    }

    public AddCollectionItemInput GetInputWithLongName(Guid collectionId)
    {
        var item = GetValidCollectionItem(collectionId);
        return new AddCollectionItemInput(
            item.CollectionId,
            new string('A', 256),
            item.Description,
            item.AcquisitionDate,
            item.Value,
            item.PhotoUrl
        );
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

    public AddCollectionItemInput GetInputWithNegativeValue(Guid collectionId)
    {
        var item = GetValidCollectionItem(collectionId);
        return new AddCollectionItemInput(
            item.CollectionId,
            item.Name,
            item.Description,
            item.AcquisitionDate,
            -10m,
            item.PhotoUrl
        );
    }
}

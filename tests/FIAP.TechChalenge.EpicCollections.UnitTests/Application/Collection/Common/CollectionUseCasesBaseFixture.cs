using Bogus;
using Moq;
using FIAP.TechChalenge.EpicCollections.Application.Interfaces;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;
using FIAP.TechChalenge.EpicCollections.UnitTests.Common;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.Common;

public class CollectionUseCasesBaseFixture : BaseFixture
{
    public Mock<ICollectionRepository> GetRepositoryMock() => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();

    public string GetValidName()
    {
        var name = "";
        while (name.Length < 3)
            name = Faker.Commerce.ProductName();
        if (name.Length > 255)
            name = name[..255];
        return name;
    }

    public string GetValidDescription()
        => Faker.Commerce.ProductDescription();

    public decimal GetValidValue()
        => Faker.Random.Decimal(0.1m, 1000m);

    public DateTime GetValidAcquisitionDate()
        => Faker.Date.Past();

    public DomainEntity.Collection.Collection GetValidCollection(Guid? collectionId = null)
        => new(
            collectionId ?? Guid.NewGuid(),
            GetValidName(),
            GetValidDescription(),
            Category.ActionFigures
        );

    public Category GetValidCategory()
    => Faker.PickRandom<Category>();

    public AddCollectionItemInput GetValidInput(Guid collectionId)
        => new(
            collectionId,
            GetValidName(),
            GetValidDescription(),
            GetValidAcquisitionDate(),
            GetValidValue(),
            Faker.Internet.Url()
        );

    public AddCollectionItemInput GetInputWithInvalidName(Guid collectionId)
    {
        var input = GetValidInput(collectionId);
        input.Name = string.Empty;
        return input;
    }

    public AddCollectionItemInput GetInputWithShortName(Guid collectionId)
    {
        var input = GetValidInput(collectionId);
        input.Name = "ab";
        return input;
    }

    public AddCollectionItemInput GetInputWithLongName(Guid collectionId)
    {
        var input = GetValidInput(collectionId);
        input.Name = new string('a', 256);
        return input;
    }

    public AddCollectionItemInput GetInputWithNegativeValue(Guid collectionId)
    {
        var input = GetValidInput(collectionId);
        input.Value = -1;
        return input;
    }
}

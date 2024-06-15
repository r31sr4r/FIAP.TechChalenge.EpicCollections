using Bogus;
using Moq;
using FIAP.TechChalenge.EpicCollections.Application.Interfaces;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;
using FIAP.TechChalenge.EpicCollections.UnitTests.Common;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.Common;

public class CollectionUseCasesBaseFixture : BaseFixture
{
    public Mock<ICollectionRepository> GetRepositoryMock() => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new();

    public string GetValidCollectionName()
    {
        var collectionName = "";
        while (collectionName.Length < 3)
            collectionName = Faker.Commerce.ProductName();
        if (collectionName.Length > 255)
            collectionName = collectionName[..255];
        return collectionName;
    }

    public string GetValidDescription()
    {
        var description = Faker.Commerce.ProductDescription();
        if (description.Length > 1000)
            description = description[..1000];
        return description;
    }

    public string GetValidName()
    {
        var name = "";
        while (name.Length < 3)
            name = Faker.Commerce.ProductName();
        if (name.Length > 255)
            name = name[..255];
        return name;
    }

    public Category GetValidCategory()
    {
        return Faker.PickRandom<Category>();
    }

    public DomainEntity.Collection GetValidCollection(Guid? userId = null)
        => new(
            userId ?? Guid.NewGuid(),
            GetValidCollectionName(),
            GetValidDescription(),
            GetValidCategory()
        );

    public List<DomainEntity.Collection> GetCollectionsList(Guid? userId = null, int length = 10)
    {
        var actualUserId = userId ?? Guid.NewGuid();
        return Enumerable.Range(1, length)
            .Select(_ => GetValidCollection(actualUserId))
            .ToList();
    }
}

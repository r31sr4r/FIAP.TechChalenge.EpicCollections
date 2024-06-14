using Bogus;
using FIAP.TechChalenge.EpicCollections.IntegrationTests.Base;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.Common;

public class CollectionUseCasesBaseFixture : BaseFixture
{
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
        => Faker.Lorem.Sentence(10);

    public Category GetValidCategory()
        => Faker.PickRandom<Category>();

    public DomainEntity.Collection GetValidCollection(Guid userId)
        => new(
            userId,
            GetValidName(),
            GetValidDescription(),
            GetValidCategory()
        );

    public List<DomainEntity.Collection> GetCollectionsList(int length = 10)
    {
        var userId = Guid.NewGuid();
        return Enumerable.Range(1, length)
            .Select(_ => GetValidCollection(userId))
            .ToList();
    }
}

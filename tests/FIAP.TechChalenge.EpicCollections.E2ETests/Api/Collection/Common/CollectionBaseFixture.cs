using Bogus;
using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;
using FIAP.TechChalenge.EpicCollections.E2ETests.Base;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using System.Threading.Tasks;
using Bogus.Extensions.Brazil;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;

[CollectionDefinition(nameof(CollectionBaseFixture))]
public class CollectionBaseFixtureCollection : ICollectionFixture<CollectionBaseFixture> { }

public class CollectionBaseFixture : BaseFixture
{
    public CollectionPersistence Persistence { get; private set; }
    public DomainEntity.User AuthenticatedUser { get; private set; }

    public CollectionBaseFixture() : base()
    {
        Persistence = new CollectionPersistence(CreateDbContext());
    }

    public async Task Authenticate()
    {
        AuthenticatedUser = GetValidUser();
        var existingUser = await Persistence.GetUserByEmail(AuthenticatedUser.Email);
        if (existingUser == null)
        {
            await Persistence.InsertUser(AuthenticatedUser);
        }
        else
        {
            AuthenticatedUser = existingUser;
        }
        await ApiClient.AuthenticateAsync(AuthenticatedUser.Email, "ValidPassword123!");
    }

    public string GetValidUserName()
    {
        var userName = "";
        while (userName.Length < 3)
            userName = Faker.Internet.UserName();
        if (userName.Length > 255)
            userName = userName[..255];
        return userName;
    }

    public string GetValidEmail() => Faker.Internet.Email();

    public string GetValidPhone()
    {
        var phoneNumber = Faker.Random.Bool()
            ? Faker.Phone.PhoneNumber("(##) ####-####")
            : Faker.Phone.PhoneNumber("(##) #####-####");

        return phoneNumber;
    }

    public string GetValidCPF() => Faker.Person.Cpf();

    public string GetValidRG() => Faker.Person.Random.AlphaNumeric(9);

    public DateTime GetValidDateOfBirth() => Faker.Person.DateOfBirth;

    public string GetValidPassword() => "ValidPassword123!";

    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;

    public DomainEntity.User GetValidUser() => new(
        GetValidUserName(),
        GetValidEmail(),
        GetValidPhone(),
        GetValidCPF(),
        GetValidDateOfBirth(),
        GetValidRG(),
        GetValidPassword()
    );

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

    public Category GetValidCategory() => Faker.PickRandom<Category>();

    public DomainEntity.Collection GetValidCollection(Guid userId)
        => new(
            userId,
            GetValidCollectionName(),
            GetValidDescription(),
            GetValidCategory()
        );

    public List<DomainEntity.Collection> GetCollectionsList(Guid userId, int length = 10)
    {
        return Enumerable.Range(1, length)
            .Select(_ => GetValidCollection(userId))
            .ToList();
    }

    public string GetInvalidShortName() => Faker.Commerce.ProductName()[..2];

    public string GetInvalidTooLongName()
    {
        var invalidTooLongName = Faker.Commerce.ProductName();
        while (invalidTooLongName.Length <= 255)
            invalidTooLongName = $"{invalidTooLongName} {Faker.Commerce.ProductName()}";
        return invalidTooLongName;
    }

    public string GetInvalidDescription() => "";

    public Category GetInvalidCategory() => (Category)999; // This simulates an invalid category.
}

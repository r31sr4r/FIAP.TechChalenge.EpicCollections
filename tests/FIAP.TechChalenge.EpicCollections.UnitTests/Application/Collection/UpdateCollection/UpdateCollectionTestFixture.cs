using Bogus;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollection;
using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;
using FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.Common;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.UpdateCollection;
[CollectionDefinition(nameof(UpdateCollectionTestFixture))]
public class UpdateCollectionTestFixtureCollection
    : ICollectionFixture<UpdateCollectionTestFixture>
{ }

public class UpdateCollectionTestFixture
    : CollectionUseCasesBaseFixture
{
    public UpdateCollectionInput GetValidInput(Guid? id = null, Guid? userId = null)
        => new UpdateCollectionInput(
            id ?? Guid.NewGuid(),
            GetValidName(),
            GetValidDescription(),
            GetValidCategory(),
            userId ?? Guid.NewGuid()
        );

    public UpdateCollectionInput GetInputWithInvalidDescription()
    {
        var input = GetValidInput();
        input.Description = string.Empty;
        return input;
    }

    public UpdateCollectionInput GetInputWithInvalidCategory()
    {
        var input = GetValidInput();
        input.Category = (Category)999; // Invalid category
        return input;
    }

    public UpdateCollectionInput GetInvalidInputShortName()
    {
        var input = GetValidInput();
        input.Name = input.Name[..2];
        return input;
    }

    public UpdateCollectionInput GetInvalidInputTooLongName()
    {
        var input = GetValidInput();
        while (input.Name.Length <= 255)
            input.Name = $"{input.Name} {Faker.Commerce.ProductName()}";
        return input;
    }
}

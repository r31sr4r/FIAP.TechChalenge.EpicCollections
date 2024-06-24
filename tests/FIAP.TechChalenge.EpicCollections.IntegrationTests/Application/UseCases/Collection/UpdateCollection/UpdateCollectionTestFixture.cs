using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollection;
using FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.UpdateCollection;

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

    public UpdateCollectionInput GetInvalidInputShortName()
    {
        var invalidInput = GetValidInput();
        invalidInput.Name = "a";
        return invalidInput;
    }

    public UpdateCollectionInput GetInvalidInputTooLongName()
    {
        var invalidInput = GetValidInput();
        invalidInput.Name = new string('a', 256);
        return invalidInput;
    }

    public UpdateCollectionInput GetInvalidInputWithoutDescription()
    {
        var invalidInput = GetValidInput();
        invalidInput.Description = string.Empty;
        return invalidInput;
    }

    public UpdateCollectionInput GetInvalidInputInvalidCategory()
    {
        var invalidInput = GetValidInput();
        invalidInput.Category = (Category)999;
        return invalidInput;
    }
}

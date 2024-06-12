using FIAP.TechChalenge.EpicCollections.Application.UseCases.User.CreateUser;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.User.Update;
using FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.User.Common;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.User.UpdateUser;

[CollectionDefinition(nameof(UpdateUserTestFixture))]
public class UpdateUserTestFixtureCollection
    : ICollectionFixture<UpdateUserTestFixture>
{ }

public class UpdateUserTestFixture
    : UserUseCasesBaseFixture
{
    public UpdateUserInput GetValidInput(Guid? id = null)
        => new UpdateUserInput(
                id ?? Guid.NewGuid(),
                GetValidUserName(),
                GetValidEmail(),
                GetValidPhone(),
                GetValidCPF(),
                GetValidDateOfBirth(),
                GetValidRG(),
                GetRandomBoolean()
    );


    public UpdateUserInput GetInputWithInvalidEmail()
    {
        var user = GetValidInput();
        user.Email = "invalid-email";
        return user;
    }

    public UpdateUserInput GetInputWithInvalidCPF()
    {
        var user = GetValidInput();
        user.CPF = "invalid-cpf";
        return user;
    }

    public UpdateUserInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidInput();
        invalidInputShortName.Name =
            invalidInputShortName.Name[..2];

        return invalidInputShortName;
    }

    public UpdateUserInput GetInvalidInputTooLongName()
    {
        var invalidInputTooLongName = GetValidInput();

        while (invalidInputTooLongName.Name.Length <= 255)
            invalidInputTooLongName.Name = $"{invalidInputTooLongName.Name} {Faker.Commerce.ProductName}";

        return invalidInputTooLongName;
    }

    public UpdateUserInput GetInvalidInputWithoutName()
    {
        var invalidInputWithoutName = GetValidInput();
        invalidInputWithoutName.Name = string.Empty;
        return invalidInputWithoutName;
    }

    public UpdateUserInput GetInvalidInputWithoutEmail()
    {
        var invalidInputWithoutEmail = GetValidInput();
        invalidInputWithoutEmail.Email = string.Empty;
        return invalidInputWithoutEmail;
    }

    public UpdateUserInput GetInvalidInputWithoutPhone()
    {
        var invalidInputWithoutPhone = GetValidInput();
        invalidInputWithoutPhone.Phone = string.Empty;
        return invalidInputWithoutPhone;
    }
}

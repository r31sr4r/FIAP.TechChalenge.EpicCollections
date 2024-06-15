using FluentAssertions;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollection;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.GetCollection;
[Collection(nameof(GetCollectionTestFixture))]
public class GetCollectionInputValidatorTest
{
    private readonly GetCollectionTestFixture _fixture;

    public GetCollectionInputValidatorTest(GetCollectionTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ValidationOk))]
    [Trait("Application", "GetCollection - Use Cases")]
    public void ValidationOk()
    {
        var validInput = new GetCollectionInput(Guid.NewGuid());
        var validator = new GetCollectionInputValidator();

        var result = validator.Validate(validInput);

        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().HaveCount(0);
    }


    [Fact(DisplayName = nameof(ValidationExceptionWhenIdIsEmpty))]
    [Trait("Application", "GetCollection - Use Cases")]
    public void ValidationExceptionWhenIdIsEmpty()
    {
        var invalidInput = new GetCollectionInput(Guid.Empty);
        var validator = new GetCollectionInputValidator();

        var result = validator.Validate(invalidInput);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
}

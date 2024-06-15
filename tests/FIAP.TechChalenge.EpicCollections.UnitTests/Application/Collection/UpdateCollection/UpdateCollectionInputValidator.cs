using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollection;
using FluentAssertions;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.UpdateCollection;
[Collection(nameof(UpdateCollectionTestFixture))]
public class UpdateCollectionInputValidatorTest
{
    private readonly UpdateCollectionTestFixture _fixture;

    public UpdateCollectionInputValidatorTest(UpdateCollectionTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DontAcceptWhenGuidIsEmpty))]
    [Trait("Application", "UpdateCollectionInputValidator - Use Cases")]
    public void DontAcceptWhenGuidIsEmpty()
    {
        var input = _fixture.GetValidInput(Guid.Empty);
        var validator = new UpdateCollectionInputValidator();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeFalse();
        validateResult.Errors.Should().HaveCount(1);
        validateResult.Errors[0].ErrorMessage
            .Should().Be("Id must not be empty");
    }

    [Fact(DisplayName = nameof(AcceptWhenGuidIsNotEmpty))]
    [Trait("Application", "UpdateCollectionInputValidator - Use Cases")]
    public void AcceptWhenGuidIsNotEmpty()
    {
        var input = _fixture.GetValidInput();
        var validator = new UpdateCollectionInputValidator();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeTrue();
        validateResult.Errors.Should().HaveCount(0);
    }
}

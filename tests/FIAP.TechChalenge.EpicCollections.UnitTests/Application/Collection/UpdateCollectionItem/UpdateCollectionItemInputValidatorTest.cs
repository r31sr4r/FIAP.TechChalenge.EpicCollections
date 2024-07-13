using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollectionItem;
using FluentAssertions;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.UpdateCollectionItem;
[Collection(nameof(UpdateCollectionItemTestFixture))]
public class UpdateCollectionItemInputValidatorTest
{
    private readonly UpdateCollectionItemTestFixture _fixture;

    public UpdateCollectionItemInputValidatorTest(UpdateCollectionItemTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DontAcceptWhenGuidIsEmpty))]
    [Trait("Application", "UpdateCollectionItemInputValidator - Use Cases")]
    public void DontAcceptWhenGuidIsEmpty()
    {
        var input = _fixture.GetValidInput(Guid.Empty, Guid.Empty);
        var validator = new UpdateCollectionItemInputValidator();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeFalse();
        validateResult.Errors.Should().HaveCount(2);
        validateResult.Errors[0].ErrorMessage
            .Should().Be("CollectionId must not be empty");
        validateResult.Errors[1].ErrorMessage
            .Should().Be("ItemId must not be empty");
    }

    [Fact(DisplayName = nameof(AcceptWhenGuidIsNotEmpty))]
    [Trait("Application", "UpdateCollectionItemInputValidator - Use Cases")]
    public void AcceptWhenGuidIsNotEmpty()
    {
        var input = _fixture.GetValidInput();
        var validator = new UpdateCollectionItemInputValidator();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeTrue();
        validateResult.Errors.Should().HaveCount(0);
    }
}

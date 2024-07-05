using FluentAssertions;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollectionItem;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.GetCollectionItem
{
    [Collection(nameof(GetCollectionItemTestFixture))]
    public class GetCollectionItemInputValidatorTest
    {
        private readonly GetCollectionItemTestFixture _fixture;

        public GetCollectionItemInputValidatorTest(GetCollectionItemTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(ValidationOk))]
        [Trait("Application", "GetCollectionItem - Use Cases")]
        public void ValidationOk()
        {
            var validInput = new GetCollectionItemInput(Guid.NewGuid(), Guid.NewGuid());
            var validator = new GetCollectionItemInputValidator();

            var result = validator.Validate(validInput);

            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = nameof(ValidationExceptionWhenCollectionIdIsEmpty))]
        [Trait("Application", "GetCollectionItem - Use Cases")]
        public void ValidationExceptionWhenCollectionIdIsEmpty()
        {
            var invalidInput = new GetCollectionItemInput(Guid.Empty, Guid.NewGuid());
            var validator = new GetCollectionItemInputValidator();

            var result = validator.Validate(invalidInput);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Be("'Collection Id' must not be empty.");
        }

        [Fact(DisplayName = nameof(ValidationExceptionWhenItemIdIsEmpty))]
        [Trait("Application", "GetCollectionItem - Use Cases")]
        public void ValidationExceptionWhenItemIdIsEmpty()
        {
            var invalidInput = new GetCollectionItemInput(Guid.NewGuid(), Guid.Empty);
            var validator = new GetCollectionItemInputValidator();

            var result = validator.Validate(invalidInput);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Be("'Item Id' must not be empty.");
        }
    }
}

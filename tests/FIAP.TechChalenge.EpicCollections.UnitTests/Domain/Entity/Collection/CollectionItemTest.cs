using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection;
using FluentAssertions;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Domain.Entity.Collection
{
    public class CollectionItemTest
    {
        private CollectionItem CreateValidItem(Guid collectionId) =>
            new CollectionItem(
                collectionId,
                "Valid Item Name",
                "Valid item description",
                DateTime.Now,
                100m,
                "http://example.com/photo.jpg"
            );

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "CollectionItem - Aggregates")]
        public void Instantiate()
        {
            var collectionId = Guid.NewGuid();
            var dateTimeBefore = DateTime.Now;

            var item = CreateValidItem(collectionId);
            var dateTimeAfter = DateTime.Now;

            item.Should().NotBeNull();
            item.CollectionId.Should().Be(collectionId);
            item.Name.Should().Be("Valid Item Name");
            item.Description.Should().Be("Valid item description");
            item.AcquisitionDate.Should().BeCloseTo(dateTimeBefore, TimeSpan.FromSeconds(1));
            item.Value.Should().Be(100m);
            item.PhotoUrl.Should().Be("http://example.com/photo.jpg");
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "CollectionItem - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            var collectionId = Guid.NewGuid();
            Action action = () => new CollectionItem(
                collectionId,
                name!,
                "Valid item description",
                DateTime.Now,
                100m,
                "http://example.com/photo.jpg"
            );

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "CollectionItem - Aggregates")]
        [InlineData("ab")]
        [InlineData("a")]
        public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            var collectionId = Guid.NewGuid();
            Action action = () => new CollectionItem(
                collectionId,
                invalidName,
                "Valid item description",
                DateTime.Now,
                100m,
                "http://example.com/photo.jpg"
            );

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be greater than 3 characters");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "CollectionItem - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThan255Characters()
        {
            var collectionId = Guid.NewGuid();
            var invalidName = new string('a', 256);

            Action action = () => new CollectionItem(
                collectionId,
                invalidName,
                "Valid item description",
                DateTime.Now,
                100m,
                "http://example.com/photo.jpg"
            );

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be less than 255 characters");
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "CollectionItem - Aggregates")]
        public void Update()
        {
            var collectionId = Guid.NewGuid();
            var item = CreateValidItem(collectionId);
            var updatedData = new
            {
                Name = "Updated Item Name",
                Description = "Updated item description.",
                AcquisitionDate = DateTime.Now,
                Value = 200m,
                PhotoUrl = "http://example.com/newphoto.jpg"
            };

            item.Update(
                updatedData.Name,
                updatedData.Description,
                updatedData.AcquisitionDate,
                updatedData.Value,
                updatedData.PhotoUrl
            );

            item.Name.Should().Be(updatedData.Name);
            item.Description.Should().Be(updatedData.Description);
            item.AcquisitionDate.Should().Be(updatedData.AcquisitionDate);
            item.Value.Should().Be(updatedData.Value);
            item.PhotoUrl.Should().Be(updatedData.PhotoUrl);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "CollectionItem - Methods")]
        public void UpdateErrorWhenNameIsEmpty()
        {
            var collectionId = Guid.NewGuid();
            var item = CreateValidItem(collectionId);

            Action action = () => item.Update("", "Valid description", DateTime.Now, 100m, "http://example.com/photo.jpg");

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsShorterThan4Characters))]
        [Trait("Domain", "CollectionItem - Methods")]
        public void UpdateErrorWhenNameIsShorterThan4Characters()
        {
            var collectionId = Guid.NewGuid();
            var item = CreateValidItem(collectionId);

            Action action = () => item.Update("Joh", "Valid description", DateTime.Now, 100m, "http://example.com/photo.jpg");

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be greater than 3 characters");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "CollectionItem - Methods")]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            var collectionId = Guid.NewGuid();
            var item = CreateValidItem(collectionId);
            var invalidName = new string('a', 256);

            Action action = () => item.Update(invalidName, "Valid description", DateTime.Now, 100m, "http://example.com/photo.jpg");

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be less than 255 characters");
        }
    }
}

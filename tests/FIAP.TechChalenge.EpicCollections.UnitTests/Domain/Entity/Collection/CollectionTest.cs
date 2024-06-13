﻿using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using Xunit;
using FluentAssertions;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FIAP.TechChalenge.EpicCollections.Domain.Common.Enums;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Domain.Entity.Collection
{
    public class CollectionTest
    {
        private class CollectionData
        {
            public Guid UserId { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public Category Category { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        private CollectionData GetInitialData() => new CollectionData
        {
            UserId = Guid.NewGuid(),
            Name = "My Action Figures",
            Description = "A collection of my favorite action figures.",
            Category = Category.ActionFigures,
            CreatedAt = DateTime.Now
        };

        private DomainEntity.Collection CreateCollection(CollectionData data) =>
            new DomainEntity.Collection(
                data.UserId,
                data.Name!,
                data.Description!,
                data.Category,
                data.CreatedAt
        );

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Collection - Aggregates")]
        public void Instantiate()
        {
            var validData = GetInitialData();
            var dateTimeBefore = DateTime.Now;

            var collection = CreateCollection(validData);
            var dateTimeAfter = DateTime.Now;

            collection.Should().NotBeNull();
            collection.UserId.Should().Be(validData.UserId);
            collection.Name.Should().Be(validData.Name);
            collection.Description.Should().Be(validData.Description);
            collection.Category.Should().Be(validData.Category);
            collection.CreatedAt.Should().BeCloseTo(dateTimeBefore, TimeSpan.FromSeconds(1));
            collection.CreatedAt.Should().BeCloseTo(dateTimeAfter, TimeSpan.FromSeconds(1));
        }



        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Collection - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            var data = new
            {
                UserId = Guid.NewGuid(),
                Name = name,
                Description = "A collection of my favorite action figures.",
                Category = Category.ActionFigures,
                CreatedAt = DateTime.Now
            };

            Action action = () => new DomainEntity.Collection(
                data.UserId,
                data.Name!,
                data.Description,
                data.Category,
                data.CreatedAt
                );

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Collection - Aggregates")]
        [InlineData("ab")]
        [InlineData("a")]
        public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            var data = GetInitialData();
            data.Name = invalidName;

            Action action = () => CreateCollection(data);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be greater than 3 characters");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Collection - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThan255Characters()
        {
            var invalidName = new string('a', 256);
            var data = GetInitialData();
            data.Name = invalidName;

            Action action = () => CreateCollection(data);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be less than 255 characters");
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Collection - Aggregates")]
        public void Update()
        {
            var initialData = GetInitialData();
            var collection = CreateCollection(initialData);
            var updatedData = new
            {
                Name = "Updated Collection",
                Description = "Updated description.",
                Category = Category.Camisetas
            };

            collection.Update(
                updatedData.Name,
                updatedData.Description,
                updatedData.Category
            );

            collection.Name.Should().Be(updatedData.Name);
            collection.Description.Should().Be(updatedData.Description);
            collection.Category.Should().Be(updatedData.Category);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Collection - Methods")]
        public void UpdateErrorWhenNameIsEmpty()
        {
            var initialData = GetInitialData();
            var collection = CreateCollection(initialData);

            Action action = () => collection.Update("", initialData.Description, initialData.Category);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsShorterThan4Characters))]
        [Trait("Domain", "Collection - Methods")]
        public void UpdateErrorWhenNameIsShorterThan4Characters()
        {
            var initialData = GetInitialData();
            var collection = CreateCollection(initialData);

            Action action = () => collection.Update("Joh", initialData.Description, initialData.Category);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be greater than 3 characters");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Collection - Methods")]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            var invalidName = new string('a', 256);
            var initialData = GetInitialData();
            var collection = CreateCollection(initialData);

            Action action = () => collection.Update(invalidName, initialData.Description, initialData.Category);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be less than 255 characters");
        }
    }
}

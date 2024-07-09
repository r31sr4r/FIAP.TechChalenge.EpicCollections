using FluentAssertions;
using Moq;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using Xunit;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using UseCase = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollectionItems;
using FIAP.TechChalenge.EpicCollections.Application.Exceptions;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.ListCollectionItems;

[Collection(nameof(ListCollectionItemsTestFixture))]
public class ListCollectionItemsTest
{
    private readonly ListCollectionItemsTestFixture _fixture;

    public ListCollectionItemsTest(ListCollectionItemsTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ShouldReturnCollectionWithItems))]
    [Trait("Application", "ListCollectionItems - Use Cases")]
    public async void ShouldReturnCollectionWithItems()
    {
        var userId = Guid.NewGuid();
        var collection = _fixture.GetCollectionWithItems(userId);
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = _fixture.GetInput(collection.Id);

        repositoryMock.Setup(x => x.GetCollectionWithItems(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(collection);

        var useCase = new UseCase.ListCollectionItems(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(collection.Id);
        output.Name.Should().Be(collection.Name);
        output.Description.Should().Be(collection.Description);
        output.Category.Should().Be(collection.Category);
        output.CreatedAt.Should().Be(collection.CreatedAt);
        output.UserId.Should().Be(collection.UserId);
        output.Items.Should().HaveCount(collection.Items.Count);
        foreach (var item in collection.Items)
        {
            output.Items.Should().Contain(i =>
                i.Id == item.Id &&
                i.Name == item.Name &&
                i.Description == item.Description &&
                i.AcquisitionDate == item.AcquisitionDate &&
                i.Value == item.Value &&
                i.PhotoUrl == item.PhotoUrl
            );
        }

        repositoryMock.Verify(x => x.GetCollectionWithItems(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowExceptionWhenCollectionNotFound))]
    [Trait("Application", "ListCollectionItems - Use Cases")]
    public async void ThrowExceptionWhenCollectionNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = _fixture.GetInput(Guid.NewGuid());

        repositoryMock.Setup(x => x.GetCollectionWithItems(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync((DomainEntity.Collection.Collection)null);

        var useCase = new UseCase.ListCollectionItems(repositoryMock.Object);

        Func<Task> action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Collection with id {input.CollectionId} not found");

        repositoryMock.Verify(x => x.GetCollectionWithItems(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }
}

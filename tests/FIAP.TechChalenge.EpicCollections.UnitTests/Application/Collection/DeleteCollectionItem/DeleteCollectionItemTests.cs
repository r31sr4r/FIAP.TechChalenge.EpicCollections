using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollectionItem;
using Moq;
using Xunit;
using UseCases = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollectionItem;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FluentAssertions;
using FIAP.TechChalenge.EpicCollections.Application.Exceptions;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.DeleteCollectionItem;

[Collection(nameof(DeleteCollectionItemTestFixture))]
public class DeleteCollectionItemTests
{
    private readonly DeleteCollectionItemTestFixture _fixture;

    public DeleteCollectionItemTests(DeleteCollectionItemTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteCollectionItem))]
    [Trait("Application", "DeleteCollectionItem - Use Cases")]
    public async Task DeleteCollectionItem()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var collection = _fixture.GetValidCollection(Guid.NewGuid());
        var item = _fixture.GetValidCollectionItem(collection.Id);
        collection.AddItem(item);

        repositoryMock.Setup(x => x.Get(collection.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(collection);

        var useCase = new UseCases.DeleteCollectionItem(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var input = _fixture.GetValidInput(collection.Id, item.Id);

        await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.DeleteItemFromCollection(
                collection.Id,
                item.Id,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ThrowWhenItemNotFound))]
    [Trait("Application", "DeleteCollectionItem - Use Cases")]
    public async Task ThrowWhenItemNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var collection = _fixture.GetValidCollection(Guid.NewGuid());
        var nonExistentItemId = Guid.NewGuid();

        repositoryMock.Setup(x => x.Get(
            collection.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(collection);

        repositoryMock.Setup(x => x.DeleteItemFromCollection(
            collection.Id,
            nonExistentItemId,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(
            new NotFoundException($"Collection item with id {nonExistentItemId} not found in collection {collection.Id}")
        );

        var useCase = new UseCases.DeleteCollectionItem(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var input = _fixture.GetValidInput(collection.Id, nonExistentItemId);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();


        repositoryMock.Verify(
            repository => repository.DeleteItemFromCollection(
                collection.Id,
                nonExistentItemId,
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
    }

}

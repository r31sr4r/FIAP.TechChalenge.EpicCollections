using FluentAssertions;
using Moq;
using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollectionItem;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using UseCases = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollectionItem;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.UpdateCollectionItem;
[Collection(nameof(UpdateCollectionItemTestFixture))]
public class UpdateCollectionItemTest
{
    private readonly UpdateCollectionItemTestFixture _fixture;

    public UpdateCollectionItemTest(UpdateCollectionItemTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(UpdateCollectionItem))]
    [Trait("Application", "UpdateCollectionItem - Use Cases")]
    [MemberData(
        nameof(UpdateCollectionItemTestDataGenerator.GetItemsToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCollectionItemTestDataGenerator)
    )]
    public async Task UpdateCollectionItem(
        DomainEntity.Collection.Collection collectionExample,
        UpdateCollectionItemInput input
    )
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(repository => repository.GetCollectionWithItems(
            collectionExample.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(collectionExample);
        var useCase = new UseCases.UpdateCollectionItem(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        CollectionItemModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.AcquisitionDate.Should().Be(input.AcquisitionDate);
        output.Value.Should().Be(input.Value);
        output.PhotoUrl.Should().Be(input.PhotoUrl);

        repositoryMock.Verify(
            repository => repository.GetCollectionWithItems(
                collectionExample.Id,
                It.IsAny<CancellationToken>()
                ), Times.Once
        );
        repositoryMock.Verify(
            repository => repository.Update(
                collectionExample,
                It.IsAny<CancellationToken>()
                ), Times.Once
        );
        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.Commit(
                It.IsAny<CancellationToken>()
                ), Times.Once
        );
    }

    [Fact(DisplayName = nameof(ThrowWhenCollectionNotFound))]
    [Trait("Application", "UpdateCollectionItem - Use Cases")]
    public async Task ThrowWhenCollectionNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = _fixture.GetValidInput();
        repositoryMock.Setup(repository => repository.GetCollectionWithItems(
            input.CollectionId,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(new NotFoundException($"Collection with id {input.CollectionId} not found"));
        var useCase = new UseCases.UpdateCollectionItem(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(
            repository => repository.GetCollectionWithItems(
                input.CollectionId,
                It.IsAny<CancellationToken>()
                ), Times.Once
        );
    }

    [Fact(DisplayName = nameof(ThrowWhenItemNotFound))]
    [Trait("Application", "UpdateCollectionItem - Use Cases")]
    public async Task ThrowWhenItemNotFound()
    {
        var collection = _fixture.GetValidCollection();
        var input = _fixture.GetValidInput(collection.Id);
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(repository => repository.GetCollectionWithItems(
            collection.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(collection);
        var useCase = new UseCases.UpdateCollectionItem(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Collection item with id {input.ItemId} not found");

        repositoryMock.Verify(
            repository => repository.GetCollectionWithItems(
                collection.Id,
                It.IsAny<CancellationToken>()
                ), Times.Once
        );
    }
}

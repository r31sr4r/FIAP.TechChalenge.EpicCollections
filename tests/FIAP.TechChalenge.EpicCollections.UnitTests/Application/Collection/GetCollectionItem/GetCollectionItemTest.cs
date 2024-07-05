using FluentAssertions;
using Moq;
using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using Xunit;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using UseCases = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollectionItem;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.GetCollectionItem
{
    [Collection(nameof(GetCollectionItemTestFixture))]
    public class GetCollectionItemTest
    {
        private readonly GetCollectionItemTestFixture _fixture;

        public GetCollectionItemTest(GetCollectionItemTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(GetCollectionItem))]
        [Trait("Application", "GetCollectionItem - Use Cases")]
        public async Task GetCollectionItem()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var exampleCollection = _fixture.GetValidCollection(Guid.NewGuid());
            var exampleItem = _fixture.GetValidCollectionItem(exampleCollection.Id);
            exampleCollection.AddItem(exampleItem);

            repositoryMock.Setup(repository => repository.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleCollection);

            var input = new UseCases.GetCollectionItemInput(exampleCollection.Id, exampleItem.Id);
            var useCase = new UseCases.GetCollectionItem(repositoryMock.Object);

            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(repository => repository.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);

            output.Should().NotBeNull();
            output.Id.Should().Be(exampleItem.Id);
            output.Name.Should().Be(exampleItem.Name);
            output.Description.Should().Be(exampleItem.Description);
            output.AcquisitionDate.Should().Be(exampleItem.AcquisitionDate);
            output.Value.Should().Be(exampleItem.Value);
            output.PhotoUrl.Should().Be(exampleItem.PhotoUrl);
        }

        [Fact(DisplayName = nameof(NotFoundExceptionWhenCollectionDoesntExist))]
        [Trait("Application", "GetCollectionItem - Use Cases")]
        public async Task NotFoundExceptionWhenCollectionDoesntExist()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var collectionId = Guid.NewGuid();
            var itemId = Guid.NewGuid();

            repositoryMock.Setup(repository => repository.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            )).ThrowsAsync(
                new NotFoundException($"Collection '{collectionId}' not found")
            );

            var input = new UseCases.GetCollectionItemInput(collectionId, itemId);
            var useCase = new UseCases.GetCollectionItem(repositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Collection '{collectionId}' not found");

            repositoryMock.Verify(repository => repository.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Fact(DisplayName = nameof(NotFoundExceptionWhenItemDoesntExist))]
        [Trait("Application", "GetCollectionItem - Use Cases")]
        public async Task NotFoundExceptionWhenItemDoesntExist()
        {
            var repositoryMock = _fixture.GetRepositoryMock();
            var exampleCollection = _fixture.GetValidCollection(Guid.NewGuid());
            var itemId = Guid.NewGuid();

            repositoryMock.Setup(repository => repository.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleCollection);

            var input = new UseCases.GetCollectionItemInput(exampleCollection.Id, itemId);
            var useCase = new UseCases.GetCollectionItem(repositoryMock.Object);

            var task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Collection item with id {itemId} not found in collection {exampleCollection.Id}");

            repositoryMock.Verify(repository => repository.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}

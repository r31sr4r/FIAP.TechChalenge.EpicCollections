using FluentAssertions;
using Moq;
using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using Xunit;
using UseCases = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollection;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.DeleteCollection;

[Collection(nameof(DeleteCollectionTestFixture))]
public class DeleteCollectionTest
{
    private readonly DeleteCollectionTestFixture _fixture;

    public DeleteCollectionTest(DeleteCollectionTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCollection))]
    [Trait("Application", "DeleteCollection - Use Cases")]
    public async Task DeleteCollection()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var collectionExample = _fixture.GetValidCollection();

        repositoryMock.Setup(repository => repository.Get(
                    collectionExample.Id,
                    It.IsAny<CancellationToken>())
        ).ReturnsAsync(collectionExample);
        var input = new UseCases.DeleteCollectionInput(collectionExample.Id);
        var useCase = new UseCases.DeleteCollection
            (repositoryMock.Object,
            unitOfWorkMock.Object
        );

        await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Get(
                collectionExample.Id,
                It.IsAny<CancellationToken>()
            ), Times.Once
        );

        repositoryMock.Verify(
            repository => repository.Delete(
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
    [Trait("Application", "DeleteCollection - Use Cases")]
    public async Task ThrowWhenCollectionNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var collectionGuid = Guid.NewGuid();
        repositoryMock.Setup(repository => repository.Get(
                    collectionGuid,
                    It.IsAny<CancellationToken>())
        ).ThrowsAsync(
            new NotFoundException($"Collection '{collectionGuid}' not found")
        );
        var input = new UseCases.DeleteCollectionInput(collectionGuid);
        var useCase = new UseCases.DeleteCollection
            (repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(
            repository => repository.Get(
                collectionGuid,
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
    }
}

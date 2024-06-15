using FluentAssertions;
using Moq;
using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollection;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using UseCases = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollection;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.UpdateCollection;
[Collection(nameof(UpdateCollectionTestFixture))]
public class UpdateCollectionTest
{
    private readonly UpdateCollectionTestFixture _fixture;

    public UpdateCollectionTest(UpdateCollectionTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(UpdateCollection))]
    [Trait("Application", "UpdateCollection - Use Cases")]
    [MemberData(
        nameof(UpdateCollectionTestDataGenerator.GetCollectionsToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCollectionTestDataGenerator)
    )]
    public async Task UpdateCollection(
        DomainEntity.Collection collectionExample,
        UpdateCollectionInput input
    )
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(repository => repository.Get(
                        collectionExample.Id,
                        It.IsAny<CancellationToken>())
               ).ReturnsAsync(collectionExample);
        var useCase = new UseCases.UpdateCollection
            (repositoryMock.Object,
                       unitOfWorkMock.Object
                              );

        CollectionModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.Category.Should().Be(input.Category);

        repositoryMock.Verify(
            repository => repository.Get(
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
    [Trait("Application", "UpdateCollection - Use Cases")]
    public async Task ThrowWhenCollectionNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = _fixture.GetValidInput();
        repositoryMock.Setup(repository => repository.Get(
            input.Id,
            It.IsAny<CancellationToken>())
            ).ThrowsAsync(new NotFoundException($"Collection '{input.Id}' not found"));
        var useCase = new UseCases.UpdateCollection(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(
            repository => repository.Get(
                input.Id,
                It.IsAny<CancellationToken>()
                ), Times.Once
        );
    }

    [Theory(DisplayName = nameof(ThrowWhenCantUpdateCollection))]
    [Trait("Application", "UpdateCollection - Use Cases")]
    [MemberData(
    nameof(UpdateCollectionTestDataGenerator.GetInvalidInputs),
    parameters: 12,
    MemberType = typeof(UpdateCollectionTestDataGenerator)
)]
    public async Task ThrowWhenCantUpdateCollection(
        UpdateCollectionInput input,
        string expectedMessage
    )
    {
        var collection = _fixture.GetValidCollection();
        input.Id = collection.Id;
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        repositoryMock.Setup(repository => repository.Get(
                collection.Id,
                It.IsAny<CancellationToken>())
        ).ReturnsAsync(collection);
        var useCase = new UseCases.UpdateCollection
            (repositoryMock.Object,
            unitOfWorkMock.Object);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);

        repositoryMock.Verify(
            repository => repository.Get(
                collection.Id,
                It.IsAny<CancellationToken>()
                ), Times.Once
        );
    }
}

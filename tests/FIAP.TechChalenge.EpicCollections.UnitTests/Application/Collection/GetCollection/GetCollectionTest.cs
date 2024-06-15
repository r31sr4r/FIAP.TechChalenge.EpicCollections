using FluentAssertions;
using Moq;
using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using Xunit;
using UseCases = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollection;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.GetCollection;

[Collection(nameof(GetCollectionTestFixture))]
public class GetCollectionTest
{
    private readonly GetCollectionTestFixture _fixture;

    public GetCollectionTest(GetCollectionTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCollection))]
    [Trait("Application", "GetCollection - Use Cases")]
    public async Task GetCollection()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleCollection = _fixture.GetValidCollection(Guid.NewGuid());
        repositoryMock.Setup(repository => repository.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCollection);
        var input = new UseCases.GetCollectionInput(exampleCollection.Id);
        var useCase = new UseCases.GetCollection(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository => repository.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleCollection.Id);
        output.UserId.Should().Be(exampleCollection.UserId);
        output.Name.Should().Be(exampleCollection.Name);
        output.Description.Should().Be(exampleCollection.Description);
        output.Category.Should().Be(exampleCollection.Category);
        output.CreatedAt.Should().Be(exampleCollection.CreatedAt);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenCollectionDoesntExist))]
    [Trait("Application", "GetCollection - Use Cases")]
    public async Task NotFoundExceptionWhenCollectionDoesntExist()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleGuid = Guid.NewGuid();
        repositoryMock.Setup(repository => repository.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(
            new NotFoundException($"Collection '{exampleGuid}' not found")
        );

        var input = new UseCases.GetCollectionInput(exampleGuid);
        var useCase = new UseCases.GetCollection(repositoryMock.Object);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Collection '{input.Id}' not found");

        repositoryMock.Verify(repository => repository.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

}

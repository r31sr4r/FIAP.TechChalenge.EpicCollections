using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using UseCases = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;


namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.AddCollectionItem;
[Collection(nameof(AddCollectionItemTestFixture))]
public class AddCollectionItemTests
{
    private readonly AddCollectionItemTestFixture _fixture;

    public AddCollectionItemTests(AddCollectionItemTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(AddCollectionItem))]
    [Trait("Application", "AddCollectionItem - Use Cases")]
    public async Task AddCollectionItem()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var collection = _fixture.GetValidCollection();

        repositoryMock.Setup(x => x.Get(collection.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(collection);

        var useCase = new UseCases.AddCollectionItem(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var input = _fixture.GetValidInput(collection.Id);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Get(
                collection.Id,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(
            unitOfWork => unitOfWork.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.Value.Should().Be(input.Value);
        output.CollectionId.Should().Be(input.CollectionId);
        output.Id.Should().NotBeEmpty();
        output.AcquisitionDate.Should().BeCloseTo(input.AcquisitionDate, TimeSpan.FromSeconds(1));
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCollectionItem))]
    [Trait("Application", "AddCollectionItem - Use Cases")]
    [MemberData(
        nameof(AddCollectionItemTestDataGenerator.GetInvalidInputs),
        parameters: 4,
        MemberType = typeof(AddCollectionItemTestDataGenerator)
    )]
    public async Task ThrowWhenCantInstantiateCollectionItem(
        AddCollectionItemInput input,
        string expectedMessage
    )
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var collection = _fixture.GetValidCollection(input.CollectionId);

        repositoryMock.Setup(x => x.Get(input.CollectionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(collection);

        var useCase = new UseCases.AddCollectionItem(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);
    }
}

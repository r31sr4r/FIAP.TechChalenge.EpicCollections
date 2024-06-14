using FluentAssertions;
using Moq;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.CreateCollection;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using Xunit;
using UseCases = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.CreateCollection;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.CreateCollection;

[Collection(nameof(CreateCollectionTestFixture))]
public class CreateCollectionTest
{
    private readonly CreateCollectionTestFixture _fixture;

    public CreateCollectionTest(CreateCollectionTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(CreateCollection))]
    [Trait("Application", "Create Collection - Use Cases")]
    public async Task CreateCollection()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var userId = Guid.NewGuid();

        var useCase = new UseCases.CreateCollection(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var input = _fixture.GetInput(userId);

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<DomainEntity.Collection>(),
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
        output.Category.Should().Be(input.Category);
        output.UserId.Should().Be(input.UserId);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCollection))]
    [Trait("Application", "Create Collection - Use Cases")]
    [MemberData(
        nameof(CreateCollectionTestDataGenerator.GetInvalidInputs),
        parameters: 4,
        MemberType = typeof(CreateCollectionTestDataGenerator)
    )]
    public async Task ThrowWhenCantInstantiateCollection(
        CreateCollectionInput input,
        string expectedMessage
    )
    {
        var useCase = new UseCases.CreateCollection(
            _fixture.GetRepositoryMock().Object,
            _fixture.GetUnitOfWorkMock().Object
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);
    }

}



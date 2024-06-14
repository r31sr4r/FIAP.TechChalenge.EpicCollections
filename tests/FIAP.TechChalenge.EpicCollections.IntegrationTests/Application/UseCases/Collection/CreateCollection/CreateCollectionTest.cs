using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.CreateCollection;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UseCase = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.CreateCollection;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.CreateCollection;

[Collection(nameof(CreateCollectionTestFixture))]
public class CreateCollectionTest
{
    private readonly CreateCollectionTestFixture _fixture;

    public CreateCollectionTest(CreateCollectionTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCollection))]
    [Trait("Integration/Application", "Create Collection - Use Cases")]
    public async Task CreateCollection()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.CreateCollection(
            repository, unitOfWork
        );

        var input = _fixture.GetInput(Guid.NewGuid());
        var currentDateTime = DateTime.Now;

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCollection = await dbContext.Collections
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == output.Id);

        dbCollection.Should().NotBeNull();
        dbCollection!.Name.Should().Be(input.Name);
        dbCollection.Description.Should().Be(input.Description);
        dbCollection.Category.Should().Be(input.Category);
        dbCollection.UserId.Should().Be(input.UserId);
        dbCollection.CreatedAt.Should().BeCloseTo(currentDateTime, TimeSpan.FromSeconds(10));

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.Category.Should().Be(input.Category);
        output.UserId.Should().Be(input.UserId);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().BeCloseTo(currentDateTime, TimeSpan.FromSeconds(10));
    }




    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCollection))]
    [Trait("Integration/Application", "Create Collection - Use Cases")]
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
        var dbContext = _fixture.CreateDbContext();
        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.CreateCollection(
            repository, unitOfWork
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);

        var dbCollection = await dbContext.Collections
            .AsNoTracking()
            .ToListAsync();

        dbCollection.Should().BeEmpty();
    }
}


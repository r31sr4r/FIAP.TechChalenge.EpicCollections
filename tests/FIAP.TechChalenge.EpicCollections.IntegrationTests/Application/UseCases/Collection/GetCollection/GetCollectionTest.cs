using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Repositories;
using FluentAssertions;
using UseCase = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollection;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.GetCollection;

[Collection(nameof(GetCollectionTestFixture))]
public class GetCollectionTest
{
    private readonly GetCollectionTestFixture _fixture;

    public GetCollectionTest(GetCollectionTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCollection))]
    [Trait("Integration/Application", "GetCollection - Use Cases")]
    public async Task GetCollection()
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleCollection = _fixture.GetValidCollection();
        dbContext.Add(exampleCollection);
        dbContext.SaveChanges();
        var collectionRepository = new CollectionRepository(dbContext);

        var input = new UseCase.GetCollectionInput(exampleCollection.Id);
        var useCase = new UseCase.GetCollection(collectionRepository);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCollection = await (_fixture.CreateDbContext(true))
            .Collections
            .FindAsync(exampleCollection.Id);

        dbCollection.Should().NotBeNull();
        dbCollection!.Name.Should().Be(exampleCollection.Name);
        dbCollection.Description.Should().Be(exampleCollection.Description);
        dbCollection.Category.Should().Be(exampleCollection.Category);
        dbCollection.CreatedAt.Should().Be(exampleCollection.CreatedAt);
        dbCollection.UserId.Should().Be(exampleCollection.UserId);
        dbCollection.Id.Should().Be(exampleCollection.Id);

        output.Should().NotBeNull();
        output!.Name.Should().Be(exampleCollection.Name);
        output.Description.Should().Be(exampleCollection.Description);
        output.Category.Should().Be(exampleCollection.Category);
        output.CreatedAt.Should().Be(exampleCollection.CreatedAt);
        output.UserId.Should().Be(exampleCollection.UserId);
        output.Id.Should().Be(exampleCollection.Id);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenCollectionDoesntExist))]
    [Trait("Integration/Application", "GetCollection - Use Cases")]
    public async Task NotFoundExceptionWhenCollectionDoesntExist()
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleCollection = _fixture.GetValidCollection();
        dbContext.Add(exampleCollection);
        dbContext.SaveChanges();
        var collectionRepository = new CollectionRepository(dbContext);
        var input = new UseCase.GetCollectionInput(Guid.NewGuid());
        var useCase = new UseCase.GetCollection(collectionRepository);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None
        );

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Collection with id {input.Id} not found");
    }
}

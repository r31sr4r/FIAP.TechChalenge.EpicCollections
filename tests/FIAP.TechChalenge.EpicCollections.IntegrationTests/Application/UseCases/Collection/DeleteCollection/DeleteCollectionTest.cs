using FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Repositories;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using UseCase = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollection;
using FluentAssertions;
using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Application.Interfaces;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork;
using FIAP.TechChalenge.EpicCollections.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.DeleteCollection;
[Collection(nameof(DeleteCollectionTestFixture))]
public class DeleteCollectionTest
{
    private readonly DeleteCollectionTestFixture _fixture;

    public DeleteCollectionTest(DeleteCollectionTestFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact(DisplayName = nameof(DeleteCollection))]
    [Trait("Integration/Application", "DeleteCollection - Use Cases")]
    public async Task DeleteCollection()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var userId = Guid.NewGuid();
        var collectionExample = _fixture.GetValidCollection(userId);
        await dbContext.AddRangeAsync(_fixture.GetCollectionsList(userId, 10));
        var tracking = await dbContext.AddAsync(collectionExample);
        dbContext.SaveChanges();
        tracking.State = EntityState.Detached;
        var useCase = new UseCase.DeleteCollection(repository, unitOfWork);
        var input = new UseCase.DeleteCollectionInput(collectionExample.Id, userId);

        await useCase.Handle(input, CancellationToken.None);

        var dbCollection = await (_fixture.CreateDbContext(true))
            .Collections
            .FindAsync(collectionExample.Id);

        dbCollection.Should().BeNull();
    }

    [Fact(DisplayName = nameof(ThrowWhenCollectionNotFound))]
    [Trait("Integration/Application", "DeleteCollection - Use Cases")]
    public async Task ThrowWhenCollectionNotFound()
    {
        var dbContext = _fixture.CreateDbContext();
        var unitOfWork = new UnitOfWork(dbContext);
        var userId = Guid.NewGuid();
        var collectionExample = _fixture.GetValidCollection(userId);
        dbContext.Add(collectionExample);
        dbContext.SaveChanges();
        var collectionRepository = new CollectionRepository(dbContext);
        var input = new UseCase.DeleteCollectionInput(Guid.NewGuid(), userId);
        var useCase = new UseCase.DeleteCollection(collectionRepository, unitOfWork);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Collection with id {input.Id} not found");
    }

    [Fact(DisplayName = nameof(ThrowWhenNotOwner))]
    [Trait("Integration/Application", "DeleteCollection - Use Cases")]
    public async Task ThrowWhenNotOwner()
    {
        var dbContext = _fixture.CreateDbContext();
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        var exampleCollections = _fixture.GetCollectionsList(userId, 10);
        var collectionExample = exampleCollections.First();
        await dbContext.AddRangeAsync(exampleCollections);
        dbContext.SaveChanges();
        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new UseCase.DeleteCollection(repository, unitOfWork);

        var input = new UseCase.DeleteCollectionInput(
            collectionExample.Id,
            differentUserId
        );

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("You are not the owner of this collection.");

        var dbCollection = await (_fixture.CreateDbContext(true))
            .Collections.FindAsync(collectionExample.Id);

        dbCollection.Should().NotBeNull();
        dbCollection!.Name.Should().Be(collectionExample.Name);
        dbCollection.Description.Should().Be(collectionExample.Description);
        dbCollection.Category.Should().Be(collectionExample.Category);
    }

}

using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollectionItem;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UseCase = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollectionItem;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.DeleteCollectionItem;

[Collection(nameof(DeleteCollectionItemTestFixture))]
public class DeleteCollectionItemTests
{
    private readonly DeleteCollectionItemTestFixture _fixture;

    public DeleteCollectionItemTests(DeleteCollectionItemTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCollectionItem))]
    [Trait("Integration/Application", "Delete CollectionItem - Use Cases")]
    public async Task DeleteCollectionItem()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.DeleteCollectionItem(
            repository, unitOfWork
        );

        var collection = _fixture.GetValidCollection();
        var item = _fixture.GetValidCollectionItem(collection.Id);
        collection.AddItem(item);
        await dbContext.Collections.AddAsync(collection);
        await dbContext.SaveChangesAsync();

        var input = _fixture.GetValidInput(collection.Id, item.Id);

        await useCase.Handle(input, CancellationToken.None);

        var newDbContext = _fixture.CreateDbContext(true); // Use a new context to verify the result
        var dbCollection = await newDbContext.Collections
            .Include(c => c.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == collection.Id);

        dbCollection.Should().NotBeNull();
        dbCollection!.Items.Should().BeEmpty();
    }


    [Fact(DisplayName = nameof(ThrowWhenCollectionNotFound))]
    [Trait("Integration/Application", "DeleteCollection - Use Cases")]
    public async Task ThrowWhenCollectionNotFound()
    {
        var dbContext = _fixture.CreateDbContext();
        var unitOfWork = new UnitOfWork(dbContext);
        var userId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var collectionExample = _fixture.GetValidCollection(userId);
        dbContext.Add(collectionExample);
        dbContext.SaveChanges();
        var collectionRepository = new CollectionRepository(dbContext);
        var input = new UseCase.DeleteCollectionItemInput(collectionExample.Id, itemId);
        var useCase = new UseCase.DeleteCollectionItem(collectionRepository, unitOfWork);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Collection item with id {input.ItemId} not found in collection {input.CollectionId}");
    }
}

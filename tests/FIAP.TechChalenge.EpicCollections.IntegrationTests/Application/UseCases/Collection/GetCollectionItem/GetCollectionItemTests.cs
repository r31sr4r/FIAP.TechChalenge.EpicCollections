using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Repositories;
using FluentAssertions;
using UseCase = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollectionItem;
using Xunit;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.GetCollectionItem;

[Collection(nameof(GetCollectionItemTestFixture))]
public class GetCollectionItemTests
{
    private readonly GetCollectionItemTestFixture _fixture;

    public GetCollectionItemTests(GetCollectionItemTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCollectionItem))]
    [Trait("Integration/Application", "GetCollectionItem - Use Cases")]
    public async Task GetCollectionItem()
    {
        var dbContext = _fixture.CreateDbContext();
        var collection = _fixture.GetValidCollection();
        var item = _fixture.GetValidCollectionItem(collection.Id);
        collection.AddItem(item);
        await dbContext.AddAsync(collection);
        await dbContext.SaveChangesAsync();
        var collectionRepository = new CollectionRepository(dbContext);

        var input = new UseCase.GetCollectionItemInput(collection.Id, item.Id);
        var useCase = new UseCase.GetCollectionItem(collectionRepository);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbItem = await dbContext.CollectionItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == item.Id);

        dbItem.Should().NotBeNull();
        dbItem!.Name.Should().Be(item.Name);
        dbItem.Description.Should().Be(item.Description);
        dbItem.AcquisitionDate.Should().Be(item.AcquisitionDate);
        dbItem.Value.Should().Be(item.Value);
        dbItem.PhotoUrl.Should().Be(item.PhotoUrl);
        dbItem.CollectionId.Should().Be(item.CollectionId);

        output.Should().NotBeNull();
        output.Name.Should().Be(item.Name);
        output.Description.Should().Be(item.Description);
        output.AcquisitionDate.Should().Be(item.AcquisitionDate);
        output.Value.Should().Be(item.Value);
        output.PhotoUrl.Should().Be(item.PhotoUrl);
    }

    [Fact(DisplayName = nameof(NotFoundExceptionWhenItemDoesntExist))]
    [Trait("Integration/Application", "GetCollectionItem - Use Cases")]
    public async Task NotFoundExceptionWhenItemDoesntExist()
    {
        var dbContext = _fixture.CreateDbContext();
        var collection = _fixture.GetValidCollection();
        await dbContext.AddAsync(collection);
        await dbContext.SaveChangesAsync();
        var collectionRepository = new CollectionRepository(dbContext);

        var input = new UseCase.GetCollectionItemInput(collection.Id, Guid.NewGuid());
        var useCase = new UseCase.GetCollectionItem(collectionRepository);

        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Collection item with id {input.ItemId} not found in collection {input.CollectionId}");
    }
}

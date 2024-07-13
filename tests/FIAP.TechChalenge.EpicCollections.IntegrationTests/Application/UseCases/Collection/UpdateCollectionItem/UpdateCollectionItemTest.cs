using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollectionItem;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Repositories;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using UseCase = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollectionItem;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FIAP.TechChalenge.EpicCollections.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.UpdateCollectionItem;

[Collection(nameof(UpdateCollectionItemTestFixture))]
public class UpdateCollectionItemTest
{
    private readonly UpdateCollectionItemTestFixture _fixture;

    public UpdateCollectionItemTest(UpdateCollectionItemTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(UpdateCollectionItem))]
    [Trait("Integration/Application", "UpdateCollectionItem - Use Cases")]
    [MemberData(
        nameof(UpdateCollectionItemTestDataGenerator.GetItemsToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCollectionItemTestDataGenerator)
    )]
    public async Task UpdateCollectionItem(
        DomainEntity.Collection.Collection collectionExample,
        UpdateCollectionItemInput input
    )
    {
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetCollectionsList());
        var trackingInfo = await dbContext.AddAsync(collectionExample);
        dbContext.SaveChanges();
        trackingInfo.State = EntityState.Detached;
        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new UseCase.UpdateCollectionItem(repository, unitOfWork);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCollection = await (_fixture.CreateDbContext(true))
            .Collections.Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == collectionExample.Id);

        var dbItem = dbCollection?.Items.FirstOrDefault(i => i.Id == input.ItemId);

        dbItem.Should().NotBeNull();
        dbItem!.Name.Should().Be(input.Name);
        dbItem.Description.Should().Be(input.Description);
        dbItem.AcquisitionDate.Should().Be(input.AcquisitionDate);
        dbItem.Value.Should().Be(input.Value);
        dbItem.PhotoUrl.Should().Be(input.PhotoUrl);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.AcquisitionDate.Should().Be(input.AcquisitionDate);
        output.Value.Should().Be(input.Value);
        output.PhotoUrl.Should().Be(input.PhotoUrl);
    }

    [Fact(DisplayName = nameof(ThrowWhenCollectionNotFound))]
    [Trait("Integration/Application", "UpdateCollectionItem - Use Cases")]
    public async Task ThrowWhenCollectionNotFound()
    {
        var input = _fixture.GetValidInput();
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetCollectionsList());
        dbContext.SaveChanges();
        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new UseCase.UpdateCollectionItem(repository, unitOfWork);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Collection with id {input.CollectionId} not found");
    }

    [Fact(DisplayName = nameof(ThrowWhenItemNotFound))]
    [Trait("Integration/Application", "UpdateCollectionItem - Use Cases")]
    public async Task ThrowWhenItemNotFound()
    {
        var collection = _fixture.GetValidCollection();
        var input = _fixture.GetValidInput(collection.Id);
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetCollectionsList());
        await dbContext.AddAsync(collection);
        dbContext.SaveChanges();
        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new UseCase.UpdateCollectionItem(repository, unitOfWork);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Collection item with id {input.ItemId} not found");
    }
}


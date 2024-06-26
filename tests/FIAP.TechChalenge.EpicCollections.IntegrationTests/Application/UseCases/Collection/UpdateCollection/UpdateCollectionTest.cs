using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollection;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Repositories;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;
using UseCase = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollection;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FIAP.TechChalenge.EpicCollections.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.UpdateCollection;

[Collection(nameof(UpdateCollectionTestFixture))]
public class UpdateCollectionTest
{
    private readonly UpdateCollectionTestFixture _fixture;

    public UpdateCollectionTest(UpdateCollectionTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(UpdateCollection))]
    [Trait("Integration/Application", "UpdateCollection - Use Cases")]
    [MemberData(
        nameof(UpdateCollectionTestDataGenerator.GetCollectionsToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCollectionTestDataGenerator)
    )]
    public async Task UpdateCollection(
        DomainEntity.Collection.Collection collectionExample,
        UpdateCollectionInput input
    )
    {
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetCollectionsList());
        var trackingInfo = await dbContext.AddAsync(collectionExample);
        dbContext.SaveChanges();
        trackingInfo.State = EntityState.Detached;
        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new UseCase.UpdateCollection(repository, unitOfWork);

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCollection = await (_fixture.CreateDbContext(true))
            .Collections.FindAsync(output.Id);

        dbCollection.Should().NotBeNull();
        dbCollection!.Name.Should().Be(input.Name);
        dbCollection.Description.Should().Be(input.Description);
        dbCollection.Category.Should().Be(input.Category);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.Category.Should().Be(input.Category);
    }

    [Fact(DisplayName = nameof(ThrowWhenCollectionNotFound))]
    [Trait("Integration/Application", "UpdateCollection - Use Cases")]
    public async Task ThrowWhenCollectionNotFound()
    {
        var input = _fixture.GetValidInput();
        var dbContext = _fixture.CreateDbContext();
        await dbContext.AddRangeAsync(_fixture.GetCollectionsList());
        dbContext.SaveChanges();
        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new UseCase.UpdateCollection(repository, unitOfWork);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Collection with id {input.Id} not found");
    }

    [Theory(DisplayName = nameof(ThrowWhenCantUpdateCollection))]
    [Trait("Integration/Application", "UpdateCollection - Use Cases")]
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
        var dbContext = _fixture.CreateDbContext();
        var exampleCollections = _fixture.GetCollectionsList();
        await dbContext.AddRangeAsync(exampleCollections);
        dbContext.SaveChanges();
        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new UseCase.UpdateCollection(repository, unitOfWork);
        input.Id = exampleCollections[0].Id;
        input.UserId = exampleCollections[0].UserId;

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);
    }

    [Fact(DisplayName = nameof(ThrowWhenNotOwner))]
    [Trait("Integration/Application", "UpdateCollection - Use Cases")]
    public async Task ThrowWhenNotOwner()
    {
        var dbContext = _fixture.CreateDbContext();
        var exampleCollections = _fixture.GetCollectionsList();
        var collectionExample = exampleCollections.First();
        var differentUserId = Guid.NewGuid();
        await dbContext.AddRangeAsync(exampleCollections);
        dbContext.SaveChanges();

        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        var useCase = new UseCase.UpdateCollection(repository, unitOfWork);

        var input = new UpdateCollectionInput(
            collectionExample.Id,
            "Updated Name",
            "Updated Description",
            collectionExample.Category,
            differentUserId
        );

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("You are not the owner of this collection.");

        var dbCollection = await (_fixture.CreateDbContext(true))
            .Collections.FindAsync(collectionExample.Id);

        dbCollection.Should().NotBeNull();
        dbCollection!.Name.Should().NotBe(input.Name);
        dbCollection.Description.Should().NotBe(input.Description);
        dbCollection.Category.Should().Be(collectionExample.Category);
    }


}

using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UseCase = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.AddCollectionItem;

[Collection(nameof(AddCollectionItemTestFixture))]
public class AddCollectionItemTests
{
    private readonly AddCollectionItemTestFixture _fixture;

    public AddCollectionItemTests(AddCollectionItemTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(AddCollectionItem))]
    [Trait("Integration/Application", "Add CollectionItem - Use Cases")]
    public async Task AddCollectionItem()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.AddCollectionItem(
            repository, unitOfWork
        );

        var collection = _fixture.GetValidCollection();
        await dbContext.Collections.AddAsync(collection);
        await dbContext.SaveChangesAsync();

        var input = _fixture.GetValidInput(collection.Id);
        var currentDateTime = DateTime.Now;

        var output = await useCase.Handle(input, CancellationToken.None);

        var dbCollection = await dbContext.Collections
            .Include(c => c.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == collection.Id);

        dbCollection.Should().NotBeNull();
        dbCollection!.Items.Should().ContainSingle();

        var dbItem = dbCollection.Items.First();
        dbItem.Name.Should().Be(input.Name);
        dbItem.Description.Should().Be(input.Description);
        dbItem.Value.Should().Be(input.Value);
        dbItem.AcquisitionDate.Should().BeCloseTo(currentDateTime, TimeSpan.FromSeconds(10));

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.Value.Should().Be(input.Value);
        output.CollectionId.Should().Be(input.CollectionId);
        output.Id.Should().NotBeEmpty();
        output.AcquisitionDate.Should().BeCloseTo(currentDateTime, TimeSpan.FromSeconds(10));
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCollectionItem))]
    [Trait("Integration/Application", "Add CollectionItem - Use Cases")]
    [MemberData(
        nameof(AddCollectionItemTestDataGenerator.GetInvalidInputs),
        parameters: 5,
        MemberType = typeof(AddCollectionItemTestDataGenerator)
    )]
    public async Task ThrowWhenCantInstantiateCollectionItem(
        AddCollectionItemInput input,
        string expectedMessage
    )
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CollectionRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.AddCollectionItem(
            repository, unitOfWork
        );

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(expectedMessage);

        var dbCollectionItems = await dbContext.Collections
            .Include(c => c.Items)
            .AsNoTracking()
            .ToListAsync();

        dbCollectionItems.Should().BeEmpty();
    }
}

using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Repositories;
using UseCase = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollectionItems;
using FluentAssertions;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.ListCollectionItems;

[Collection(nameof(ListCollectionItemsTestFixture))]
public class ListCollectionItemsTest
{
    private readonly ListCollectionItemsTestFixture _fixture;

    public ListCollectionItemsTest(ListCollectionItemsTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "ShouldReturnCollectionWithItems")]
    [Trait("Integration/Application", "ListCollectionItems - Use Cases")]
    public async Task ShouldReturnCollectionWithItems()
    {
        EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
        var exampleCollection = _fixture.GetCollectionWithItems();
        await dbContext.AddAsync(exampleCollection);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var collectionRepository = new CollectionRepository(dbContext);
        var input = new UseCase.ListCollectionItemsInput(exampleCollection.Id, 1, 10, "", "", SearchOrder.Asc);
        var useCase = new UseCase.ListCollectionItems(collectionRepository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleCollection.Id);
        output.Name.Should().Be(exampleCollection.Name);
        output.Description.Should().Be(exampleCollection.Description);
        output.Category.Should().Be(exampleCollection.Category);
        output.CreatedAt.Should().Be(exampleCollection.CreatedAt);
        output.UserId.Should().Be(exampleCollection.UserId);
        output.Items.Should().HaveCount(exampleCollection.Items.Count);
        foreach (var item in exampleCollection.Items)
        {
            output.Items.Should().Contain(i =>
                i.Id == item.Id &&
                i.Name == item.Name &&
                i.Description == item.Description &&
                i.AcquisitionDate == item.AcquisitionDate &&
                i.Value == item.Value &&
                i.PhotoUrl == item.PhotoUrl
            );
        }
    }

    [Fact(DisplayName = "ThrowExceptionWhenCollectionNotFound")]
    [Trait("Integration/Application", "ListCollectionItems - Use Cases")]
    public async Task ThrowExceptionWhenCollectionNotFound()
    {
        EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
        var collectionRepository = new CollectionRepository(dbContext);
        var input = new UseCase.ListCollectionItemsInput(Guid.NewGuid(), 1, 10, "", "", SearchOrder.Asc);
        var useCase = new UseCase.ListCollectionItems(collectionRepository);

        Func<Task> action = async () => await useCase.Handle(input, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Collection with id {input.CollectionId} not found");
    }
}

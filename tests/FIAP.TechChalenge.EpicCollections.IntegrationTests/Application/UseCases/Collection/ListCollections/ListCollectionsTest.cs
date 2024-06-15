using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Repositories;
using UseCase = FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollections;
using FluentAssertions;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.Application.Exceptions;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.ListCollections;

[Collection(nameof(ListCollectionsTestFixture))]
public class ListCollectionsTest
{
    private readonly ListCollectionsTestFixture _fixture;

    public ListCollectionsTest(ListCollectionsTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = "SearchReturnsListAndTotal")]
    [Trait("Integration/Application", "ListCollections - Use Cases")]
    public async Task SearchReturnsListAndTotal()
    {
        EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
        var exampleCollectionList = _fixture.GetCollectionsList(null, 15);
        await dbContext.AddRangeAsync(exampleCollectionList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var collectionRepository = new CollectionRepository(dbContext);
        var searchInput = new UseCase.ListCollectionsInput(page: 1, perPage: 10);
        var useCase = new UseCase.ListCollections(collectionRepository);

        var output = await useCase.Handle(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleCollectionList.Count);
        output.Items.Should().HaveCount(10);
        foreach (CollectionModelOutput outputItem in output.Items)
        {
            var exampleItem = exampleCollectionList.Find(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem!.Id.Should().Be(exampleItem!.Id);
            outputItem.Name.Should().Be(exampleItem.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.Category.Should().Be(exampleItem.Category);
            outputItem.UserId.Should().Be(exampleItem.UserId);
        }
    }

    [Fact(DisplayName = "SearchReturnsEmpty")]
    [Trait("Integration/Application", "ListCollections - Use Cases")]
    public async Task SearchReturnsEmpty()
    {
        EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
        var collectionRepository = new CollectionRepository(dbContext);
        var searchInput = new UseCase.ListCollectionsInput(page: 1, perPage: 10);
        var useCase = new UseCase.ListCollections(collectionRepository);

        var output = await useCase.Handle(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = "SearchReturnsPaginated")]
    [Trait("Integration/Application", "ListCollections - Use Cases")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchReturnsPaginated(
        int itemsToGenerate,
        int page,
        int perPage,
        int expectedTotal
    )
    {
        EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
        var exampleCollectionList = _fixture.GetCollectionsList(null, itemsToGenerate);
        await dbContext.AddRangeAsync(exampleCollectionList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var collectionRepository = new CollectionRepository(dbContext);
        var searchInput = new UseCase.ListCollectionsInput(page, perPage);
        var useCase = new UseCase.ListCollections(collectionRepository);

        var output = await useCase.Handle(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleCollectionList.Count);
        output.Items.Should().HaveCount(expectedTotal);
        foreach (CollectionModelOutput outputItem in output.Items)
        {
            var exampleItem = exampleCollectionList.Find(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem!.Id.Should().Be(exampleItem!.Id);
            outputItem.Name.Should().Be(exampleItem.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.Category.Should().Be(exampleItem.Category);
            outputItem.UserId.Should().Be(exampleItem.UserId);
        }
    }

    [Theory(DisplayName = "SearchByText")]
    [Trait("Integration/Application", "ListCollections - Use Cases")]
    [InlineData("Collection 1", 1, 5, 2, 2)]
    [InlineData("Collection 2", 1, 5, 2, 2)]
    [InlineData("Example", 1, 5, 3, 3)]
    [InlineData("Example", 2, 5, 3, 0)]
    [InlineData("Example", 3, 5, 3, 0)]
    public async Task SearchByText(
        string search,
        int page,
        int perPage,
        int expectedTotalResult,
        int expectedTotalItems
    )
    {
        EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
        var exampleCollectionList = _fixture.GetExampleCollectionsListWithNames(
            new List<string>()
            {
            "Example Collection 1",
            "Example Collection 2",
            "Collection 1",
            "Collection 2",
            "Example Collection 3",
            }
        );
        await dbContext.AddRangeAsync(exampleCollectionList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var collectionRepository = new CollectionRepository(dbContext);
        var searchInput = new UseCase.ListCollectionsInput(page, perPage, search);
        var useCase = new UseCase.ListCollections(collectionRepository);

        var output = await useCase.Handle(searchInput, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(expectedTotalResult);
        output.Items.Should().HaveCount(expectedTotalItems);
        foreach (CollectionModelOutput outputItem in output.Items)
        {
            var exampleItem = exampleCollectionList.Find(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem!.Id.Should().Be(exampleItem!.Id);
            outputItem.Name.Should().Be(exampleItem.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.Category.Should().Be(exampleItem.Category);
            outputItem.UserId.Should().Be(exampleItem.UserId);
        }
    }


    [Theory(DisplayName = "SearchOrdered")]
    [Trait("Integration/Application", "ListCollections - Use Cases")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    public async Task SearchOrdered(
        string orderBy,
        string order
    )
    {
        EpicCollectionsDbContext dbContext = _fixture.CreateDbContext();
        var exampleCollectionList = _fixture.GetCollectionsList(null,10);
        await dbContext.AddRangeAsync(exampleCollectionList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var collectionRepository = new CollectionRepository(dbContext);
        var searchOrder = order == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var searchInput = new UseCase.ListCollectionsInput(
            page: 1,
            perPage: 20,
            "",
            orderBy,
            searchOrder
        );
        var useCase = new UseCase.ListCollections(collectionRepository);

        var output = await useCase.Handle(searchInput, CancellationToken.None);

        var expectOrdered = _fixture.SortList(exampleCollectionList, orderBy, searchOrder);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Page.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleCollectionList.Count);
        output.Items.Should().HaveCount(exampleCollectionList.Count);

        for (int i = 0; i < output.Items.Count; i++)
        {
            var outputItem = output.Items[i];
            var exampleItem = expectOrdered[i];
            outputItem.Should().NotBeNull();
            outputItem!.Id.Should().Be(exampleItem.Id);
            outputItem.Name.Should().Be(exampleItem.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.Category.Should().Be(exampleItem.Category);
            outputItem.UserId.Should().Be(exampleItem.UserId);
        }
    }
}

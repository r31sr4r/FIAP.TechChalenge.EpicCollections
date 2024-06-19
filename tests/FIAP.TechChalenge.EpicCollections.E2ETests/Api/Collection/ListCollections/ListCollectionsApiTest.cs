using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollections;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.E2ETests.Extensions.DateTime;
using FIAP.TechChalenge.EpicCollections.E2ETests.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.ListCollections;

[Collection(nameof(ListCollectionsApiTestFixture))]
public class ListCollectionsApiTest : IDisposable
{
    private readonly ListCollectionsApiTestFixture _fixture;

    public ListCollectionsApiTest(ListCollectionsApiTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ListCollectionsAndTotalByDefault))]
    [Trait("E2E/Api", "Collection/List - Endpoints")]
    public async Task ListCollectionsAndTotalByDefault()
    {
        var defaultPerPage = 15;

        // Inserir um usuário válido
        var validUser = _fixture.GetValidUser();
        await _fixture.Persistence.InsertUser(validUser);

        var exampleCollectionsList = _fixture.GetCollectionsList(validUser.Id, 20);
        await _fixture.Persistence.InsertList(exampleCollectionsList);

        var (response, output) = await _fixture
            .ApiClient
            .Get<TestApiResponseList<CollectionModelOutput>>($"/collections");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output.Meta.Should().NotBeNull();
        output.Meta.Total.Should().Be(exampleCollectionsList.Count);
        output.Data.Should().HaveCount(defaultPerPage);
        output!.Data.Should().NotBeNull();
        foreach (CollectionModelOutput collection in output.Data)
        {
            var dbCollection = exampleCollectionsList
                .FirstOrDefault(x => x.Id == collection.Id);
            dbCollection.Should().NotBeNull();
            dbCollection!.Name.Should().Be(collection.Name);
            dbCollection.Description.Should().Be(collection.Description);
            dbCollection.Category.Should().Be(collection.Category);
            dbCollection.UserId.Should().Be(collection.UserId);
            dbCollection.CreatedAt.TrimMilliSeconds().Should().BeSameDateAs(
                collection.CreatedAt.TrimMilliSeconds()
            );
        }
    }

    [Fact(DisplayName = nameof(ItemsEmptyWhenPersistenceEmpty))]
    [Trait("E2E/Api", "Collection/List - Endpoints")]
    public async Task ItemsEmptyWhenPersistenceEmpty()
    {
        var (response, output) = await _fixture
            .ApiClient
            .Get<TestApiResponseList<CollectionModelOutput>>("/collections");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Should().HaveCount(0);
        output.Meta.Should().NotBeNull();
        output.Meta.Total.Should().Be(0);
        output.Data.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(ListCollectionsAndTotal))]
    [Trait("E2E/Api", "Collection/List - Endpoints")]
    public async Task ListCollectionsAndTotal()
    {
        var validUser = _fixture.GetValidUser();
        await _fixture.Persistence.InsertUser(validUser);

        var exampleCollectionsList = _fixture.GetCollectionsList(validUser.Id, 20);
        await _fixture.Persistence.InsertList(exampleCollectionsList);
        var input = new ListCollectionsInput
        {
            Page = 1,
            PerPage = 5
        };

        var (response, output) = await _fixture
            .ApiClient
            .Get<TestApiResponseList<CollectionModelOutput>>("/collections", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Meta.Total.Should().Be(exampleCollectionsList.Count);
        foreach (var item in output!.Data)
        {
            var exampleCollection = exampleCollectionsList
                .FirstOrDefault(x => x.Id == item.Id);
            item.Name.Should().Be(exampleCollection!.Name);
            item.Description.Should().Be(exampleCollection!.Description);
            item.Category.Should().Be(exampleCollection!.Category);
            item.UserId.Should().Be(exampleCollection!.UserId);
            item.CreatedAt.TrimMilliSeconds().Should().BeSameDateAs(
                exampleCollection!.CreatedAt.TrimMilliSeconds()
            );
        }
    }

    [Theory(DisplayName = "ListPaginated")]
    [Trait("E2E/Api", "Collection/List - Endpoints")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 3, 5, 0)]
    public async Task ListPaginated(
        int itemsToGenerate,
        int page,
        int perPage,
        int expectedTotal
        )
    {
        var validUser = _fixture.GetValidUser();
        await _fixture.Persistence.InsertUser(validUser);

        var exampleCollectionsList = _fixture.GetCollectionsList(validUser.Id, itemsToGenerate);
        await _fixture.Persistence.InsertList(exampleCollectionsList);
        var input = new ListCollectionsInput
        {
            Page = page,
            PerPage = perPage
        };

        var (response, output) = await _fixture
            .ApiClient
            .Get<TestApiResponseList<CollectionModelOutput>>("/collections", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Meta.Total.Should().Be(exampleCollectionsList.Count);
        foreach (var item in output!.Data)
        {
            var exampleCollection = exampleCollectionsList
                .FirstOrDefault(x => x.Id == item.Id);
            item.Name.Should().Be(exampleCollection!.Name);
            item.Description.Should().Be(exampleCollection!.Description);
            item.Category.Should().Be(exampleCollection!.Category);
            item.UserId.Should().Be(exampleCollection!.UserId);
            item.CreatedAt.TrimMilliSeconds().Should().BeSameDateAs(
                exampleCollection!.CreatedAt.TrimMilliSeconds()
            );
        }
    }

    [Theory(DisplayName = "SearchByText")]
    [Trait("E2E/Api", "Collection/List - Endpoints")]
    [InlineData("Collection 1", 1, 5, 1, 1)]
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
        var validUser = _fixture.GetValidUser();
        await _fixture.Persistence.InsertUser(validUser);

        var exampleCollectionsList = _fixture.GetExampleCollectionsListWithNames(
            validUser.Id,
            new List<string>()
            {
                "Example Collection 1",
                "Example Collection 2",
                "Collection 1",
                "Collection 2",
                "Example Collection 3",
            }
        );

        await _fixture.Persistence.InsertList(exampleCollectionsList);
        var input = new ListCollectionsInput
        {
            Page = page,
            PerPage = perPage,
            Search = search
        };

        var (response, output) = await _fixture
            .ApiClient
            .Get<TestApiResponseList<CollectionModelOutput>>("/collections", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        foreach (var item in output!.Data)
        {
            var exampleCollection = exampleCollectionsList
                .FirstOrDefault(x => x.Id == item.Id);
            item.Name.Should().Be(exampleCollection!.Name);
            item.Description.Should().Be(exampleCollection!.Description);
            item.Category.Should().Be(exampleCollection!.Category);
            item.UserId.Should().Be(exampleCollection!.UserId);
            item.CreatedAt.TrimMilliSeconds().Should().BeSameDateAs(
                exampleCollection!.CreatedAt.TrimMilliSeconds()
            );
        }
    }

    [Theory(DisplayName = "SearchOrdered")]
    [Trait("E2E/Api", "Collection/List - Endpoints")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("createdAt", "asc")]
    [InlineData("createdAt", "desc")]
    public async Task SearchOrdered(
      string orderBy,
      string order
  )
    {
        var validUser = _fixture.GetValidUser();
        await _fixture.Persistence.InsertUser(validUser);

        var exampleCollectionsList = _fixture.GetCollectionsList(validUser.Id, 10);
        await _fixture.Persistence.InsertList(exampleCollectionsList);
        var inputOrder = order == "asc"
            ? SearchOrder.Asc
            : SearchOrder.Desc;

        var input = new ListCollectionsInput
        {
            Page = 1,
            PerPage = 20,
            Search = "",
            Sort = orderBy,
            Dir = inputOrder
        };

        var (response, output) = await _fixture
            .ApiClient
            .Get<TestApiResponseList<CollectionModelOutput>>("/collections", input);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Meta.Total.Should().Be(exampleCollectionsList.Count);
        var expectOrdered = _fixture.SortList(exampleCollectionsList, input.Sort, input.Dir);

        for (int i = 0; i < output!.Data.Count; i++)
        {
            var outputItem = output.Data[i];
            var exampleItem = expectOrdered[i];
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.Category.Should().Be(exampleItem.Category);
            outputItem.UserId.Should().Be(exampleItem.UserId);
            outputItem.CreatedAt.TrimMilliSeconds().Should().BeSameDateAs(
                exampleItem.CreatedAt.TrimMilliSeconds()
            );
        }
    }
 

    public void Dispose()
        => _fixture.CleanPersistence();
}

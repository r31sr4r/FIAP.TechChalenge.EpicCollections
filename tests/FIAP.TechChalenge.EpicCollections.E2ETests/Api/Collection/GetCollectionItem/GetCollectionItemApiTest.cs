using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;
using System.Net;
using Xunit;
using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Response;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.GetCollectionItem;

[Collection(nameof(GetCollectionItemApiTestFixture))]
public class GetCollectionItemApiTest : IAsyncLifetime, IDisposable
{
    private readonly GetCollectionItemApiTestFixture _fixture;

    public GetCollectionItemApiTest(GetCollectionItemApiTestFixture fixture)
        => _fixture = fixture;

    public async Task InitializeAsync()
    {
        await _fixture.Authenticate();
    }

    public Task DisposeAsync()
    {
        _fixture.CleanPersistence();
        return Task.CompletedTask;
    }

    [Fact(DisplayName = nameof(GetCollectionItem))]
    [Trait("E2E/Api", "CollectionItem/Get - Endpoints")]
    public async Task GetCollectionItem()
    {
        var (collectionId, itemId) = await _fixture.CreateCollectionWithItem();

        var (response, output) = await _fixture
            .ApiClient
            .Get<ApiResponse<CollectionItemModelOutput>>($"/collections/{collectionId}/items/{itemId}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Should().NotBeNull();
        output.Data.Id.Should().Be(itemId);
        output.Data.CollectionId.Should().Be(collectionId);
    }

    [Fact(DisplayName = nameof(ErrorWhenItemNotFound))]
    [Trait("E2E/Api", "CollectionItem/Get - Endpoints")]
    public async Task ErrorWhenItemNotFound()
    {
        var collection = _fixture.GetValidCollection(_fixture.AuthenticatedUser.Id);
        await _fixture.Persistence.Insert(collection);
        var randomItemId = Guid.NewGuid();

        var (response, output) = await _fixture
            .ApiClient
            .Get<ProblemDetails>($"/collections/{collection.Id}/items/{randomItemId}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        output.Title.Should().Be("Not found");
        output.Detail.Should().Be($"Collection item with id {randomItemId} not found in collection {collection.Id}");
        output.Type.Should().Be("NotFound");
    }

    public void Dispose()
        => _fixture.CleanPersistence();
}

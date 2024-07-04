using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;
using System.Net;
using Xunit;
using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Response;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.DeleteCollectionItem;

[Collection(nameof(DeleteCollectionItemApiTestFixture))]
public class DeleteCollectionItemApiTest : IAsyncLifetime, IDisposable
{
    private readonly DeleteCollectionItemApiTestFixture _fixture;

    public DeleteCollectionItemApiTest(DeleteCollectionItemApiTestFixture fixture)
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

    [Fact(DisplayName = nameof(DeleteCollectionItem))]
    [Trait("E2E/Api", "CollectionItem/Delete - Endpoints")]
    public async Task DeleteCollectionItem()
    {
        var (collectionId, itemId) = await _fixture.CreateCollectionWithItem();

        var (response, output) = await _fixture
            .ApiClient
            .Delete<ApiResponse<string>>($"/collections/{collectionId}/items/{itemId}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Should().Be("Item deleted successfully");

        var dbCollection = await _fixture.Persistence.GetById(collectionId);
        dbCollection.Should().NotBeNull();
        dbCollection!.Items.Should().NotContain(i => i.Id == itemId);
    }

    [Fact(DisplayName = nameof(ErrorWhenItemNotFound))]
    [Trait("E2E/Api", "CollectionItem/Delete - Endpoints")]
    public async Task ErrorWhenItemNotFound()
    {
        var collection = _fixture.GetValidCollection(_fixture.AuthenticatedUser.Id);
        await _fixture.Persistence.Insert(collection);
        var randomItemId = Guid.NewGuid();

        var (response, output) = await _fixture
            .ApiClient
            .Delete<ProblemDetails>($"/collections/{collection.Id}/items/{randomItemId}");

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

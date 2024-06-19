using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;
using System.Net;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.DeleteCollection;

[Collection(nameof(CollectionBaseFixture))]
public class DeleteCollectionApiTest : IAsyncLifetime, IDisposable
{
    private readonly CollectionBaseFixture _fixture;

    public DeleteCollectionApiTest(CollectionBaseFixture fixture)
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

    [Fact(DisplayName = nameof(DeleteCollection))]
    [Trait("E2E/Api", "Collection/Delete - Endpoints")]
    public async Task DeleteCollection()
    {
        var exampleCollectionsList = _fixture.GetCollectionsList(_fixture.AuthenticatedUser.Id, 20);
        await _fixture.Persistence.InsertList(exampleCollectionsList);
        var exampleCollection = exampleCollectionsList[10];

        var (response, output) = await _fixture
            .ApiClient
            .Delete<object>($"/collections/{exampleCollection.Id}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
        output.Should().BeNull();

        var collection = await _fixture.Persistence
            .GetById(exampleCollection.Id);
        collection.Should().BeNull();
    }

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("E2E/Api", "Collection/Delete - Endpoints")]
    public async Task ErrorWhenNotFound()
    {
        var exampleCollectionsList = _fixture.GetCollectionsList(_fixture.AuthenticatedUser.Id, 20);
        await _fixture.Persistence.InsertList(exampleCollectionsList);
        var randomGuid = Guid.NewGuid();

        var (response, output) = await _fixture
            .ApiClient
            .Delete<ProblemDetails>($"/collections/{randomGuid}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        output.Title.Should().Be("Not found");
        output.Detail.Should().Be($"Collection with id {randomGuid} not found");
        output.Type.Should().Be("NotFound");
    }

    public void Dispose()
        => _fixture.CleanPersistence();
}

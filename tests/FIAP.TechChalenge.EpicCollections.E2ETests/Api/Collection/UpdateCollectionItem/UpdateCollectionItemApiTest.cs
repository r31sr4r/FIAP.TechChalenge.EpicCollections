using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Response;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.UpdateCollectionItem;

[Collection(nameof(UpdateCollectionItemApiTestFixture))]
public class UpdateCollectionItemApiTest : IAsyncLifetime, IDisposable
{
    private readonly UpdateCollectionItemApiTestFixture _fixture;

    public UpdateCollectionItemApiTest(UpdateCollectionItemApiTestFixture fixture)
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

    [Fact(DisplayName = nameof(UpdateCollectionItem))]
    [Trait("E2E/Api", "CollectionItem/Update - Endpoints")]
    public async Task UpdateCollectionItem()
    {
        var exampleCollection = _fixture.GetCollectionWithItems(_fixture.AuthenticatedUser.Id);
        await _fixture.Persistence.Insert(exampleCollection);
        var exampleItem = exampleCollection.Items.First();
        var itemModelInput = _fixture.GetInput(exampleCollection.Id, exampleItem.Id);

        var (response, output) = await _fixture
            .ApiClient
            .Put<ApiResponse<CollectionItemModelOutput>>(
            $"/collections/{exampleCollection.Id}/items/{exampleItem.Id}",
            itemModelInput
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Name.Should().Be(itemModelInput.Name);
        output.Data.Description.Should().Be(itemModelInput.Description);
        output.Data.Id.Should().NotBeEmpty();
        output.Data.Id.Should().Be(exampleItem.Id);

        var dbCollection = await _fixture.Persistence
            .GetItemById(exampleItem.Id);
        dbCollection.Should().NotBeNull();
        dbCollection!.Name.Should().Be(itemModelInput.Name);
        dbCollection.Description.Should().Be(itemModelInput.Description);
        dbCollection.Id.Should().NotBeEmpty();
    }


    public void Dispose()
        => _fixture.CleanPersistence();
}

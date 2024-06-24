using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Response;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.UpdateCollection;

[Collection(nameof(UpdateCollectionApiTestFixture))]
public class UpdateCollectionApiTest : IAsyncLifetime, IDisposable
{
    private readonly UpdateCollectionApiTestFixture _fixture;

    public UpdateCollectionApiTest(UpdateCollectionApiTestFixture fixture)
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

    [Fact(DisplayName = nameof(UpdateCollection))]
    [Trait("E2E/Api", "Collection/Update - Endpoints")]
    public async Task UpdateCollection()
    {
        var exampleCollectionsList = _fixture.GetCollectionsList(_fixture.AuthenticatedUser.Id, 20);
        await _fixture.Persistence.InsertList(exampleCollectionsList);
        var exampleCollection = exampleCollectionsList[10];
        var collectionModelInput = _fixture.GetInput(_fixture.AuthenticatedUser.Id);

        var (response, output) = await _fixture
            .ApiClient
            .Put<ApiResponse<CollectionModelOutput>>(
            $"/collections/{exampleCollection.Id}",
            collectionModelInput
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Name.Should().Be(collectionModelInput.Name);
        output.Data.Description.Should().Be(collectionModelInput.Description);
        output.Data.Id.Should().NotBeEmpty();
        output.Data.Id.Should().Be(exampleCollection.Id);
        output.Data.CreatedAt.Should().NotBeSameDateAs(default);

        var dbCollection = await _fixture.Persistence
            .GetById(exampleCollection.Id);
        dbCollection.Should().NotBeNull();
        dbCollection!.Name.Should().Be(collectionModelInput.Name);
        dbCollection.Description.Should().Be(collectionModelInput.Description);
        dbCollection.Id.Should().NotBeEmpty();
    }

    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("E2E/Api", "Collection/Update - Endpoints")]
    public async Task ErrorWhenNotFound()
    {
        var collectionModelInput = _fixture.GetInput(_fixture.AuthenticatedUser.Id);

        var (response, output) = await _fixture
            .ApiClient
            .Put<ProblemDetails>($"/collections/{collectionModelInput.Id}", collectionModelInput);

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        output.Title.Should().Be("Not found");
        output.Detail.Should().Be($"Collection with id {collectionModelInput.Id} not found");
        output.Type.Should().Be("NotFound");
    }

    [Fact(DisplayName = nameof(ErrorWhenNotOwner))]
    [Trait("E2E/Api", "Collection/Update - Endpoints")]
    public async Task ErrorWhenNotOwner()
    {
        var differentUser = _fixture.GetValidUser();
        await _fixture.Persistence.InsertUser(differentUser);
        await _fixture.ApiClient.AuthenticateAsync(differentUser.Email, "ValidPassword123!");

        var exampleCollectionsList = _fixture.GetCollectionsList(_fixture.AuthenticatedUser.Id, 20);
        await _fixture.Persistence.InsertList(exampleCollectionsList);
        var exampleCollection = exampleCollectionsList[10];
        var collectionModelInput = _fixture.GetInput(_fixture.AuthenticatedUser.Id);

        var (response, output) = await _fixture
            .ApiClient
            .Put<ProblemDetails>(
            $"/collections/{exampleCollection.Id}",
            collectionModelInput
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        output.Should().NotBeNull();
        output!.Title.Should().Be("Forbidden");
    }


    public void Dispose()
        => _fixture.CleanPersistence();
}




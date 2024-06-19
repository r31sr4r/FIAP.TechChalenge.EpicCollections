using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Response;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.E2ETests.Extensions.DateTime;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.GetCollection;

[Collection(nameof(GetCollectionApiTestFixture))]
public class GetCollectionApiTest : IDisposable
{
    private readonly GetCollectionApiTestFixture _fixture;

    public GetCollectionApiTest(GetCollectionApiTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(GetCollection))]
    [Trait("E2E/Api", "Collection/Get - Endpoints")]
    public async Task GetCollection()
    {
        // Inserir um usuário válido
        var validUser = _fixture.GetValidUser();
        await _fixture.Persistence.InsertUser(validUser);

        // Inserir coleções para o usuário
        var exampleCollectionsList = _fixture.GetCollectionsList(validUser.Id, 20);
        await _fixture.Persistence.InsertList(exampleCollectionsList);
        var exampleCollection = exampleCollectionsList[10];

        var (response, output) = await _fixture
            .ApiClient
            .Get<ApiResponse<CollectionModelOutput>>($"/collections/{exampleCollection.Id}");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Should().NotBeNull();
        output.Data.Id.Should().Be(exampleCollection.Id);
        output.Data.Name.Should().Be(exampleCollection.Name);
        output.Data.Description.Should().Be(exampleCollection.Description);
        output.Data.Category.Should().Be(exampleCollection.Category);
        output.Data.UserId.Should().Be(exampleCollection.UserId);
        output.Data.CreatedAt.TrimMilliSeconds().Should().BeSameDateAs(
            exampleCollection.CreatedAt.TrimMilliSeconds()
        );
    }

    [Fact(DisplayName = nameof(ThrowExceptionWhenNotFound))]
    [Trait("E2E/Api", "Collection/Get - Endpoints")]
    public async Task ThrowExceptionWhenNotFound()
    {
        var validUser = _fixture.GetValidUser();
        await _fixture.Persistence.InsertUser(validUser);

        var exampleCollectionsList = _fixture.GetCollectionsList(validUser.Id, 20);
        await _fixture.Persistence.InsertList(exampleCollectionsList);
        var randomGuid = Guid.NewGuid();

        var (response, output) = await _fixture
            .ApiClient
            .Get<ProblemDetails>($"/collections/{randomGuid}");

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

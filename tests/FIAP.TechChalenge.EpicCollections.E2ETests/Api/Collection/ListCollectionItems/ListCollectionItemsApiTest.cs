using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollectionItems;
using FIAP.TechChalenge.EpicCollections.E2ETests.Extensions.DateTime;
using FIAP.TechChalenge.EpicCollections.E2ETests.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.ListCollectionItems;

[Collection(nameof(ListCollectionItemsApiTestFixture))]
public class ListCollectionItemsApiTest : IDisposable
{
    private readonly ListCollectionItemsApiTestFixture _fixture;

    public ListCollectionItemsApiTest(ListCollectionItemsApiTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(ShouldReturnCollectionWithItems))]
    [Trait("E2E/Api", "Collection/ListItems - Endpoints")]
    public async Task ShouldReturnCollectionWithItems()
    {
        // Inserir um usuário válido
        var validUser = _fixture.GetValidUser();
        await _fixture.Persistence.InsertUser(validUser);

        // Inserir coleção com itens para o usuário
        var exampleCollection = _fixture.GetCollectionWithItems(validUser.Id);
        await _fixture.Persistence.Insert(exampleCollection);

        var (response, output) = await _fixture
            .ApiClient
            .Get<TestApiResponse<CollectionModelOutput>>($"/collections/{exampleCollection.Id}/items");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Should().NotBeNull();
        var collection = output.Data;

        collection.Id.Should().Be(exampleCollection.Id);
        collection.Name.Should().Be(exampleCollection.Name);
        collection.Description.Should().Be(exampleCollection.Description);
        collection.Category.Should().Be(exampleCollection.Category);
        collection.CreatedAt.TrimMilliSeconds().Should().BeSameDateAs(
            exampleCollection.CreatedAt.TrimMilliSeconds()
        );
        collection.Items.Should().HaveCount(exampleCollection.Items.Count);
    }

    [Fact(DisplayName = nameof(ThrowExceptionWhenCollectionNotFound))]
    [Trait("E2E/Api", "Collection/ListItems - Endpoints")]
    public async Task ThrowExceptionWhenCollectionNotFound()
    {
        var (response, output) = await _fixture
            .ApiClient
            .Get<ProblemDetails>($"/collections/{Guid.NewGuid()}/items");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        output.Title.Should().Be("Not found");
    }

    public void Dispose()
        => _fixture.CleanPersistence();
}

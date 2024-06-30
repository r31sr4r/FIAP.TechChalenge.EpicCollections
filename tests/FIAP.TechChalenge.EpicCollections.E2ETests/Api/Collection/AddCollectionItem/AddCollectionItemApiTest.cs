using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Response;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Collection;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.AddCollectionItem;

[Collection(nameof(AddCollectionItemApiTestFixture))]
public class AddCollectionItemApiTest : IAsyncLifetime, IDisposable
{
    private readonly AddCollectionItemApiTestFixture _fixture;

    public AddCollectionItemApiTest(AddCollectionItemApiTestFixture fixture)
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

    [Fact(DisplayName = nameof(AddCollectionItem))]
    [Trait("E2E/Api", "Collection/AddCollectionItem - Endpoints")]
    public async Task AddCollectionItem()
    {
        var collectionId = await _fixture.CreateCollection();

        var input = _fixture.GetInput(collectionId);

        var (response, output) = await _fixture
            .ApiClient
            .Post<ApiResponse<CollectionItemModelOutput>>(
                $"/collections/{collectionId}/items",
                input
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);
        output!.Data.Should().NotBeNull();
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.Value.Should().Be(input.Value);
        output.Data.PhotoUrl.Should().Be(input.PhotoUrl);
        output.Data.AcquisitionDate.Should().BeCloseTo(input.AcquisitionDate, TimeSpan.FromSeconds(10));

        var dbItem = await _fixture.Persistence
            .GetItemById(output.Data.Id);
        dbItem.Should().NotBeNull();
        dbItem!.Name.Should().Be(input.Name);
        dbItem.Description.Should().Be(input.Description);
        dbItem.Value.Should().Be(input.Value);
        dbItem.PhotoUrl.Should().Be(input.PhotoUrl);
        dbItem.AcquisitionDate.Should().BeCloseTo(input.AcquisitionDate, TimeSpan.FromSeconds(10));
    }

    [Theory(DisplayName = nameof(ErrorWhenCantInstantiateItem))]
    [Trait("E2E/Api", "Collection/AddCollectionItem - Endpoints")]
    [MemberData(
        nameof(AddCollectionItemApiTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(AddCollectionItemApiTestDataGenerator)
    )]
    public async Task ErrorWhenCantInstantiateItem(
        AddCollectionItemInput input,
        string expectedDetail
    )
    {
        var collectionId = await _fixture.CreateCollection();
        input = new AddCollectionItemInput(collectionId, input.Name, input.Description, input.AcquisitionDate, input.Value, input.PhotoUrl);

        var (response, output) = await _fixture
            .ApiClient
            .Post<ProblemDetails>(
                $"/collections/{collectionId}/items",
                input
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        output.Should().NotBeNull();
        output!.Title.Should().Be("One or more validation errors occurred");
        output.Type.Should().Be("UnprocessableEntity");
        output.Status.Should().Be((int)StatusCodes.Status422UnprocessableEntity);
        output.Detail.Should().Be(expectedDetail);
    }

    public void Dispose()
        => _fixture.CleanPersistence();
}

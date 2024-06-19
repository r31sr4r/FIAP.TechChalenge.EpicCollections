using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Response;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.CreateCollection;
using FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.CreateColleciton;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.CreateCollection;

[Collection(nameof(CreateCollectionApiTestFixture))]
public class CreateCollectionApiTest : IAsyncLifetime, IDisposable
{
    private readonly CreateCollectionApiTestFixture _fixture;

    public CreateCollectionApiTest(CreateCollectionApiTestFixture fixture)
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

    [Fact(DisplayName = nameof(CreateCollection))]
    [Trait("E2E/Api", "Collection/Create - Endpoints")]
    public async Task CreateCollection()
    {
        var input = _fixture.GetInput();

        var (response, output) = await _fixture
            .ApiClient
            .Post<ApiResponse<CollectionModelOutput>>(
                "/collections",
                input
            );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.Created);
        output!.Data.Should().NotBeNull();
        output.Data.Name.Should().Be(input.Name);
        output.Data.Description.Should().Be(input.Description);
        output.Data.Category.Should().Be(input.Category);
        output.Data.UserId.Should().Be(input.UserId);
        output.Data.Id.Should().NotBeEmpty();
        output.Data.CreatedAt.Should().NotBeSameDateAs(default);

        var dbCollection = await _fixture.Persistence
            .GetById(output.Data.Id);
        dbCollection.Should().NotBeNull();
        dbCollection!.Name.Should().Be(input.Name);
        dbCollection.Description.Should().Be(input.Description);
        dbCollection.Category.Should().Be(input.Category);
        dbCollection.UserId.Should().Be(input.UserId);
        dbCollection.Id.Should().NotBeEmpty();
    }

    [Theory(DisplayName = nameof(ErrorWhenCantInstantiateAggregate))]
    [Trait("E2E/Api", "Collection/Create - Endpoints")]
    [MemberData(
        nameof(CreateCollectionApiTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(CreateCollectionApiTestDataGenerator)
    )]
    public async Task ErrorWhenCantInstantiateAggregate(
        CreateCollectionInput input,
        string expectedDetail
    )
    {
        var validUser = _fixture.AuthenticatedUser;
        input.UserId = validUser.Id;

        var (response, output) = await _fixture
            .ApiClient
            .Post<ProblemDetails>(
                "/collections",
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

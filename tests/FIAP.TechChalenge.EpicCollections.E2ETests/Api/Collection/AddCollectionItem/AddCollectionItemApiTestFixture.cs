using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;
using FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Collection;
using System.Text;
using System.Text.Json;
using FIAP.TechChalenge.EpicCollections.Api.ApiModels.Response;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FluentAssertions;
using System.Net;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.AddCollectionItem;

[CollectionDefinition(nameof(AddCollectionItemApiTestFixture))]
public class AddCollectionItemApiTestFixtureCollection : ICollectionFixture<AddCollectionItemApiTestFixture> { }

public class AddCollectionItemApiTestFixture : CollectionBaseFixture
{
    public AddCollectionItemInput GetInput(Guid collectionId)
    {
        return new AddCollectionItemInput(
            collectionId,
            "Item Name",
            "Item Description",
            DateTime.UtcNow,
            100.0m,
            "http://example.com/photo.jpg"
        );
    }

    public async Task<Guid> CreateCollection()
    {
        var input = GetCreateCollectionInput();
        var (response, output) = await ApiClient.Post<ApiResponse<CollectionModelOutput>>("/collections", input);

        return output.Data.Id;
    }

    public CreateCollectionApiInput GetCreateCollectionInput()
    {
        return new CreateCollectionApiInput(
            "Test Collection",
            "Test Description",
            FIAP.TechChalenge.EpicCollections.Domain.Common.Enums.Category.Camisetas
        );
    }
}

using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;
using FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.AddCollectionItem;

[CollectionDefinition(nameof(AddCollectionItemTestFixture))]
public class AddCollectionItemTestFixtureCollection : ICollectionFixture<AddCollectionItemTestFixture>
{ }

public class AddCollectionItemTestFixture : CollectionUseCasesBaseFixture
{
    public AddCollectionItemInput GetValidInput(Guid collectionId)
    {
        var item = GetValidCollectionItem(collectionId);
        return new AddCollectionItemInput(
            item.CollectionId,
            item.Name,
            item.Description,
            item.AcquisitionDate,
            item.Value,
            item.PhotoUrl
        );
    }

    public AddCollectionItemInput GetInputWithInvalidName(Guid collectionId)
    {
        var item = GetValidCollectionItem(collectionId);
        return new AddCollectionItemInput(
            item.CollectionId,
            string.Empty,
            item.Description,
            item.AcquisitionDate,
            item.Value,
            item.PhotoUrl
        );
    }

    public AddCollectionItemInput GetInputWithShortName(Guid collectionId)
    {
        var item = GetValidCollectionItem(collectionId);
        return new AddCollectionItemInput(
            item.CollectionId,
            "Ab",
            item.Description,
            item.AcquisitionDate,
            item.Value,
            item.PhotoUrl
        );
    }

    public AddCollectionItemInput GetInputWithLongName(Guid collectionId)
    {
        var item = GetValidCollectionItem(collectionId);
        return new AddCollectionItemInput(
            item.CollectionId,
            new string('A', 256),
            item.Description,
            item.AcquisitionDate,
            item.Value,
            item.PhotoUrl
        );
    }

    public AddCollectionItemInput GetInputWithNegativeValue(Guid collectionId)
    {
        var item = GetValidCollectionItem(collectionId);
        return new AddCollectionItemInput(
            item.CollectionId,
            item.Name,
            item.Description,
            item.AcquisitionDate,
            -10m,
            item.PhotoUrl
        );
    }
}

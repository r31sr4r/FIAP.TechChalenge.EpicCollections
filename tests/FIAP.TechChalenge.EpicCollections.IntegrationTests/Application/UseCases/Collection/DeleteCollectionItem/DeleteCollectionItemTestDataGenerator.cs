using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollectionItem;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.DeleteCollectionItem;

public class DeleteCollectionItemTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int numberOfIterations = 5)
    {
        var fixture = new DeleteCollectionItemTestFixture();
        var invalidInputsList = new List<object[]>();

        for (int index = 0; index < numberOfIterations; index++)
        {
            var collectionId = fixture.GetValidCollection().Id;
            var itemId = Guid.NewGuid(); // Generate a new GUID to simulate an item that doesn't exist
            invalidInputsList.Add(new object[]
            {
                new DeleteCollectionItemInput(collectionId, itemId),
                $"Collection item with id {itemId} not found in collection {collectionId}"
            });
        }
        return invalidInputsList;
    }
}

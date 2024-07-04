using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollectionItem;
using FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.DeleteCollectionItem;

public class DeleteCollectionItemTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int numberOfIterations = 4)
    {
        var fixture = new DeleteCollectionItemTestFixture();
        var invalidInputsList = new List<object[]>();

        for (int index = 0; index < numberOfIterations; index++)
        {
            var collectionId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            invalidInputsList.Add(new object[]
            {
                new DeleteCollectionItemInput(collectionId, itemId),
                $"Item with id {itemId} not found in collection {collectionId}"
            });
        }
        return invalidInputsList;
    }
}

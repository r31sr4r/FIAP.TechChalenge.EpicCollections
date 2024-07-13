namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.UpdateCollectionItem;

public class UpdateCollectionItemTestDataGenerator
{
    public static IEnumerable<object[]> GetItemsToUpdate(int times = 10)
    {
        var fixture = new UpdateCollectionItemTestFixture();
        for (int indice = 0; indice < times; indice++)
        {
            var exampleCollection = fixture.GetValidCollectionWithItems();
            var exampleInput = fixture.GetValidInput(exampleCollection.Id, exampleCollection.Items.First().Id);
            yield return new object[] { exampleCollection, exampleInput };
        }
    }
}


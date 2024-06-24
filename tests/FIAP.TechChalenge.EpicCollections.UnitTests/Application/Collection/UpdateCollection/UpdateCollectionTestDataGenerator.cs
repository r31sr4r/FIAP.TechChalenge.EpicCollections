namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.Collection.UpdateCollection;
public class UpdateCollectionTestDataGenerator
{
    public static IEnumerable<object[]> GetCollectionsToUpdate(int times = 10)
    {
        var fixture = new UpdateCollectionTestFixture();
        for (int indice = 0; indice < times; indice++)
        {
            var exampleCollection = fixture.GetValidCollection();
            var exampleInput = fixture.GetValidInput(exampleCollection.Id, exampleCollection.UserId);
            yield return new object[] { exampleCollection, exampleInput };
        }
    }

    public static IEnumerable<object[]> GetInvalidInputs(int numberOfIterations = 12)
    {
        var fixture = new UpdateCollectionTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 4;

        for (int index = 0; index < numberOfIterations; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add(new object[]
                    {
                    fixture.GetInvalidInputShortName(),
                    "Name should be greater than 3 characters"
                });
                    break;
                case 1:
                    invalidInputsList.Add(new object[]
                    {
                    fixture.GetInvalidInputTooLongName(),
                    "Name should be less than 255 characters"
                });
                    break;
                case 2:
                    invalidInputsList.Add(new object[]
                    {
                    fixture.GetInputWithInvalidDescription(),
                    "Description should not be empty or null"
                });
                    break;
                case 3:
                    invalidInputsList.Add(new object[]
                    {
                    fixture.GetInputWithInvalidCategory(),
                    "Category must be a valid value"
                });
                    break;
            }
        }
        return invalidInputsList;
    }
}

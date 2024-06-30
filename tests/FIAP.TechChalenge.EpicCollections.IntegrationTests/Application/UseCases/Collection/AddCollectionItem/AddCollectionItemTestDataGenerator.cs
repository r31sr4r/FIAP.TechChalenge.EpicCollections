using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.AddCollectionItem;

public class AddCollectionItemTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int numberOfIterations = 4)
    {
        var fixture = new AddCollectionItemTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 4;

        for (int index = 0; index < numberOfIterations; index++)
        {
            var collectionId = Guid.NewGuid();
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInputWithInvalidName(collectionId),
                        "Name should not be empty or null"
                    });
                    break;
                case 1:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInputWithShortName(collectionId),
                        "Name should be greater than 3 characters"
                    });
                    break;
                case 2:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInputWithLongName(collectionId),
                        "Name should be less than 255 characters"
                    });
                    break;
                case 3:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInputWithNegativeValue(collectionId),
                        "Value should not be negative"
                    });
                    break;
            }
        }
        return invalidInputsList;
    }
}


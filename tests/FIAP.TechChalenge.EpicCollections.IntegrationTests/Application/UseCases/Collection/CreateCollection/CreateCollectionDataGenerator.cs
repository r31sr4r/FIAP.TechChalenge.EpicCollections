namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Application.UseCases.Collection.CreateCollection;

public class CreateCollectionTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int numberOfIterations = 4)
    {
        var fixture = new CreateCollectionTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 4;

        for (int index = 0; index < numberOfIterations; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInputWithInvalidName(),
                        "Name should not be empty or null"
                    });
                    break;
                case 1:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInputWithShortName(),
                        "Name should be greater than 3 characters"
                    });
                    break;
                case 2:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInputWithLongName(),
                        "Name should be less than 255 characters"
                    });
                    break;
                case 3:
                    invalidInputsList.Add(new object[]
                    {
                        fixture.GetInputWithEmptyDescription(),
                        "Description should not be empty or null"
                    });
                    break;
            }
        }
        return invalidInputsList;
    }
}

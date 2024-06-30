using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.AddCollectionItem;

public class AddCollectionItemApiTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new AddCollectionItemApiTestFixture();
        fixture.Authenticate().GetAwaiter().GetResult();

        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 4;

        for (int index = 0; index < totalInvalidCases; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    var input1 = fixture.GetInput(Guid.NewGuid());
                    input1.Name = string.Empty;
                    invalidInputsList.Add(new object[]
                    {
                        input1,
                        "Name should not be empty or null"
                    });
                    break;
                case 1:
                    var input2 = fixture.GetInput(Guid.NewGuid());
                    input2.Name = "Ab";
                    invalidInputsList.Add(new object[]
                    {
                        input2,
                        "Name should be greater than 3 characters"
                    });
                    break;
                case 2:
                    var input3 = fixture.GetInput(Guid.NewGuid());
                    input3.Name = new string('A', 256);
                    invalidInputsList.Add(new object[]
                    {
                        input3,
                        "Name should be less than 255 characters"
                    });
                    break;
                case 3:
                    var input4 = fixture.GetInput(Guid.NewGuid());
                    input4.Value = -1;
                    invalidInputsList.Add(new object[]
                    {
                        input4,
                        "Value should not be negative"
                    });
                    break;
            }
        }
        return invalidInputsList;
    }
}

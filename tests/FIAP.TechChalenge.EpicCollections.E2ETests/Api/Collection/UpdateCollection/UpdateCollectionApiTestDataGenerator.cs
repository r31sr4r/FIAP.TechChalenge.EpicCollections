namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.Collection.UpdateCollection;
public class UpdateCollectionApiTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new UpdateCollectionApiTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 4;

        fixture.Authenticate().GetAwaiter().GetResult();

        for (int index = 0; index < totalInvalidCases; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    var input1 = fixture.GetInput(fixture.AuthenticatedUser.Id);
                    input1.Name = fixture.GetInvalidShortName();
                    invalidInputsList.Add(new object[]
                    {
                        input1,
                        "Name should be greater than 3 characters"
                    });
                    break;
                case 1:
                    var input2 = fixture.GetInput(fixture.AuthenticatedUser.Id);
                    input2.Name = fixture.GetInvalidTooLongName();
                    invalidInputsList.Add(new object[]
                    {
                        input2,
                        "Name should be less than 255 characters"
                    });
                    break;
                case 2:
                    var input3 = fixture.GetInput(fixture.AuthenticatedUser.Id);
                    input3.Description = fixture.GetInvalidDescription();
                    invalidInputsList.Add(new object[]
                    {
                        input3,
                        "Description should not be empty or null"
                    });
                    break;
                case 3:
                    var input4 = fixture.GetInput(fixture.AuthenticatedUser.Id);
                    input4.Category = fixture.GetInvalidCategory();
                    invalidInputsList.Add(new object[]
                    {
                        input4,
                        "Category must be a valid value"
                    });
                    break;
            }
        }
        return invalidInputsList;
    }
}

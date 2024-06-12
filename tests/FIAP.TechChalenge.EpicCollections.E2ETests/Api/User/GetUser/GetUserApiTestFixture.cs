using FIAP.TechChalenge.EpicCollections.E2ETests.Api.User.Common;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.User.GetUser;

[CollectionDefinition(nameof(GetUserApiTestFixture))]
public class GetUserApiTestFixtureCollection
: ICollectionFixture<GetUserApiTestFixture>
{ }

public class GetUserApiTestFixture
    : UserBaseFixture
{

}

using FIAP.TechChalenge.EpicCollections.E2ETests.Api.User.Common;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Api.User.DeleteUser;

[CollectionDefinition(nameof(DeleteUserApiTestFixture))]
public class DeleteUserApiTestFixtureCollection
: ICollectionFixture<DeleteUserApiTestFixture>
{ }

public class DeleteUserApiTestFixture
    : UserBaseFixture
{

}

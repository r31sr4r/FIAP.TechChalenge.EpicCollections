using FIAP.TechChalenge.EpicCollections.UnitTests.Application.User.Common;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.User.GetUser;
[CollectionDefinition(nameof(GetUserTestFixture))]
public class GetUserTestFixtureCollection :
    ICollectionFixture<GetUserTestFixture>
{ }

public class GetUserTestFixture
    : UserUseCasesBaseFixture
{ }

using FIAP.TechChalenge.EpicCollections.UnitTests.Application.User.Common;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.User.AuthUser;
[CollectionDefinition(nameof(AuthUserTestFixture))]
public class AuthUserTestFixtureCollection :
    ICollectionFixture<AuthUserTestFixture>
{ }

public class AuthUserTestFixture
    : UserUseCasesBaseFixture
{ }


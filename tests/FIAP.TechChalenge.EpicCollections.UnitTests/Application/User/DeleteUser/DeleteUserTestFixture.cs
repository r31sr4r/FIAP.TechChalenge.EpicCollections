using FIAP.TechChalenge.EpicCollections.UnitTests.Application.User.Common;
using Xunit;

namespace FIAP.TechChalenge.EpicCollections.UnitTests.Application.User.DeleteUser;
[CollectionDefinition(nameof(DeleteUserTestFixture))]
public class DeleteUserTestFixtureCollection
    : ICollectionFixture<DeleteUserTestFixture>
{ }
public class DeleteUserTestFixture
    : UserUseCasesBaseFixture
{ }
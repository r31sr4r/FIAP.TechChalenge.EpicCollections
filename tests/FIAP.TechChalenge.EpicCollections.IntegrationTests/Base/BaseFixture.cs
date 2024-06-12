using Bogus;
using Microsoft.EntityFrameworkCore;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;

namespace FIAP.TechChalenge.EpicCollections.IntegrationTests.Base;
public class BaseFixture
{
    protected Faker Faker { get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
    }

    public EpicCollectionsDbContext CreateDbContext(
        bool preserveData = false
    )
    {
        var context = new EpicCollectionsDbContext(
            new DbContextOptionsBuilder<EpicCollectionsDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .Options
        );

        if (!preserveData)
            context.Database.EnsureDeleted();

        return context;

    }


}
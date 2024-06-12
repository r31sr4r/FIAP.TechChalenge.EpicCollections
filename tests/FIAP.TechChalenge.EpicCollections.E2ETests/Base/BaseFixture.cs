using Bogus;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FIAP.TechChalenge.EpicCollections.E2ETests.Base;
public class BaseFixture : IDisposable
{
    protected Faker Faker { get; set; }

    public ApiClient ApiClient { get; set; }

    public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }

    private readonly string _dbConnectionString;

    public HttpClient HttpClient { get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        HttpClient = WebAppFactory.CreateClient();
        ApiClient = new ApiClient(HttpClient);
        var configuration = WebAppFactory.Services.GetService(typeof(IConfiguration));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        _dbConnectionString = ((IConfiguration)configuration).GetConnectionString("epiccollectionsdb");
    }

    public EpicCollectionsDbContext CreateDbContext()
    {
        var context = new EpicCollectionsDbContext(
             new DbContextOptionsBuilder<EpicCollectionsDbContext>()
             .UseMySql(
                 _dbConnectionString,
                 ServerVersion.AutoDetect(_dbConnectionString))
             .Options
         );
        return context;
    }

    public void CleanPersistence()
    {
        var context = CreateDbContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        WebAppFactory.Dispose();
    }
}

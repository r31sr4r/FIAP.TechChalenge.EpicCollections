using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace FIAP.TechChalenge.EpicCollections.Api.Configurations;

public static class ConnectionsConfiguration
{
    public static IServiceCollection AddAppConnections(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbConnection(configuration);
        return services;
    }

    private static IServiceCollection AddDbConnection(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("epiccollectionsdb");
        services.AddDbContext<EpicCollectionsDbContext>(
            options =>  options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            )
        );

        return services;
    }

    public static WebApplication MigrateDatabase(
       this WebApplication app)
    {
        var environment = Environment
            .GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (environment == "E2ETest") return app;
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider
            .GetRequiredService<EpicCollectionsDbContext>();
        dbContext.Database.Migrate();
        return app;
    }
}

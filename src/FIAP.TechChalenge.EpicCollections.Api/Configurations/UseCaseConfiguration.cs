using FIAP.TechChalenge.EpicCollections.Application.Interfaces;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.User.CreateUser;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF.Repositories;
using MediatR;
using FIAP.TechChalenge.EpicCollections.Infra.Data.EF;

namespace FIAP.TechChalenge.EpicCollections.Api.Configurations;

public static class UseCaseConfiguration
{
    public static IServiceCollection AddUseCases(
               this IServiceCollection services
           )
    {
        services.AddMediatR(typeof(CreateUser));
        services.AddRepositories();  

        return services;
    }

    private static IServiceCollection AddRepositories(
           this IServiceCollection services
       )
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ICollectionRepository, CollectionRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        return services;
    }


}

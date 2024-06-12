using FIAP.TechChalenge.EpicCollections.Domain.Entity;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;

namespace FIAP.TechChalenge.EpicCollections.Domain.Repository;
public interface IUserRepository
    : IGenericRepository<User>,
    ISearchableRepository<User>
{
    public Task<IReadOnlyList<Guid>> GetIdsListByIds(
        List<Guid> ids,
        CancellationToken cancellationToken
    );

    Task<User> GetByEmail(string email, CancellationToken cancellationToken);

}


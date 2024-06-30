using FIAP.TechChalenge.EpicCollections.Domain.SeedWork.SearchableRepository;
using FIAP.TechChalenge.EpicCollections.Domain.SeedWork;
using FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection;

namespace FIAP.TechChalenge.EpicCollections.Domain.Repository;
public interface ICollectionRepository
    : IGenericRepository<Collection>,
    ISearchableRepository<Collection>
{
    Task<IReadOnlyList<Collection>> GetCollectionsByUserId(Guid userId, CancellationToken cancellationToken);

    Task AddItemToCollection(CollectionItem item, CancellationToken cancellationToken);
}

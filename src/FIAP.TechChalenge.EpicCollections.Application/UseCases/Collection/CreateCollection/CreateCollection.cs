using FIAP.TechChalenge.EpicCollections.Application.Interfaces;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.CreateCollection;
public class CreateCollection : ICreateCollection
{
    private readonly ICollectionRepository _collectionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCollection(
        ICollectionRepository collectionRepository,
        IUnitOfWork unitOfWork
    )
    {
        _collectionRepository = collectionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CollectionModelOutput> Handle(CreateCollectionInput request, CancellationToken cancellationToken)
    {
        var collection = new DomainEntity.Collection.Collection(
            request.UserId,
            request.Name,
            request.Description,
            request.Category
        );

        await _collectionRepository.Insert(collection, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CollectionModelOutput.FromCollection(collection);
    }
}

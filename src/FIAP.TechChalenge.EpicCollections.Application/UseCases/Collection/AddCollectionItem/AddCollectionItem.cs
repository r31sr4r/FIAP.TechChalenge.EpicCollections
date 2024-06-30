using FIAP.TechChalenge.EpicCollections.Application.Interfaces;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;
using DomainEntity = FIAP.TechChalenge.EpicCollections.Domain.Entity.Collection;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;

public class AddCollectionItem : IAddCollectionItem
{
    private readonly ICollectionRepository _collectionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCollectionItem(
        ICollectionRepository collectionRepository,
        IUnitOfWork unitOfWork
    )
    {
        _collectionRepository = collectionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CollectionItemModelOutput> Handle(AddCollectionItemInput request, CancellationToken cancellationToken)
    {
        var collection = await _collectionRepository.Get(request.CollectionId, cancellationToken);
        if (collection == null)
        {
            throw new EntityNotFoundException($"Collection with id {request.CollectionId} not found");
        }

        var item = new DomainEntity.CollectionItem(
            request.CollectionId,
            request.Name,
            request.Description,
            request.AcquisitionDate,
            request.Value,
            request.PhotoUrl
        );

        await _collectionRepository.AddItemToCollection(item, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CollectionItemModelOutput.FromCollectionItem(item);
    }
}

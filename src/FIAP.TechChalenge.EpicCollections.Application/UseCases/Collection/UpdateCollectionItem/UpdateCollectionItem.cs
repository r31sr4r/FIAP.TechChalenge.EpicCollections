using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Application.Interfaces;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;
using MediatR;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollectionItem;
public class UpdateCollectionItem : IUpdateCollectionItem
{
    private readonly ICollectionRepository _collectionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCollectionItem(
        ICollectionRepository collectionRepository,
        IUnitOfWork unitOfWork)
    {
        _collectionRepository = collectionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CollectionItemModelOutput> Handle(UpdateCollectionItemInput request, CancellationToken cancellationToken)
    {
        var collection = await _collectionRepository.GetCollectionWithItems(request.CollectionId, cancellationToken);

        if (collection == null)
            throw new NotFoundException($"Collection with id {request.CollectionId} not found");

        var item = collection.Items.FirstOrDefault(i => i.Id == request.ItemId);

        if (item == null)
            throw new NotFoundException($"Collection item with id {request.ItemId} not found");

        item.Update(
            request.Name,
            request.Description,
            request.AcquisitionDate,
            request.Value,
            request.PhotoUrl
        );

        await _collectionRepository.Update(collection, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CollectionItemModelOutput.FromCollectionItem(item);
    }
}

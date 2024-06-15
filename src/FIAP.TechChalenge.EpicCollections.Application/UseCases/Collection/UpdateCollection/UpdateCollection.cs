using FIAP.TechChalenge.EpicCollections.Application.Interfaces;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollection;
public class UpdateCollection
    : IUpdateCollection
{
    private readonly ICollectionRepository _collectionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCollection(
        ICollectionRepository collectionRepository,
        IUnitOfWork unitOfWork)
        => (_collectionRepository, _unitOfWork)
            = (collectionRepository, unitOfWork);


    public async Task<CollectionModelOutput> Handle(UpdateCollectionInput request, CancellationToken cancellationToken)
    {
        var collection = await _collectionRepository.Get(request.Id, cancellationToken);
        collection.Update(
            request.Name,
            request.Description,
            request.Category
        );

        await _collectionRepository.Update(collection, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CollectionModelOutput.FromCollection(collection);
    }
}

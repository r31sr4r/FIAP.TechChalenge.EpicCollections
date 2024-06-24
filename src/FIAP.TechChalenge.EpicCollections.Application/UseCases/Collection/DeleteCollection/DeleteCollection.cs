using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.Interfaces;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollection;
public class DeleteCollection : IDeleteCollection
{
    private readonly ICollectionRepository _collectionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCollection(ICollectionRepository collectionRepository, IUnitOfWork unitOfWork)
        => (_collectionRepository, _unitOfWork) = (collectionRepository, unitOfWork);

    public async Task<Unit> Handle(DeleteCollectionInput request, CancellationToken cancellationToken)
    {
        var collection = await _collectionRepository.Get(request.Id, cancellationToken);
        if (collection.UserId != request.UserId)
            throw new UnauthorizedAccessException("You are not the owner of this collection.");
        await _collectionRepository.Delete(collection, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return Unit.Value;
    }
}

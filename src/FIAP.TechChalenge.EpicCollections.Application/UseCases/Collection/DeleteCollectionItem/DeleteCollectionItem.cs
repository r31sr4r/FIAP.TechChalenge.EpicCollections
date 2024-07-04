using FIAP.TechChalenge.EpicCollections.Application.Interfaces;
using FIAP.TechChalenge.EpicCollections.Domain.Exceptions;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollectionItem
{
    public class DeleteCollectionItem : IDeleteCollectionItem
    {
        private readonly ICollectionRepository _collectionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCollectionItem(
            ICollectionRepository collectionRepository,
            IUnitOfWork unitOfWork)
        {
            _collectionRepository = collectionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteCollectionItemInput request, CancellationToken cancellationToken)
        {
            await _collectionRepository.DeleteItemFromCollection(request.CollectionId, request.ItemId, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);
            return Unit.Value;
        }
    }
}

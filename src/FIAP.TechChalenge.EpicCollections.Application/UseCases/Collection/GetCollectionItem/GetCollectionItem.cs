using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollectionItem
{
    public class GetCollectionItem : IGetCollectionItem
    {
        private readonly ICollectionRepository _repository;

        public GetCollectionItem(ICollectionRepository repository)
        {
            _repository = repository;
        }

        public async Task<CollectionItemModelOutput> Handle(
            GetCollectionItemInput request,
            CancellationToken cancellationToken
        )
        {
            var collection = await _repository.Get(request.CollectionId, cancellationToken);
            var item = collection.Items.FirstOrDefault(i => i.Id == request.ItemId);

            if (item == null)
                throw new NotFoundException($"Collection item with id {request.ItemId} not found in collection {request.CollectionId}");

            return CollectionItemModelOutput.FromCollectionItem(item);
        }
    }
}

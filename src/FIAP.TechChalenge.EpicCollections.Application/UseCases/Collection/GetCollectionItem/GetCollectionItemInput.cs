using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollectionItem
{
    public class GetCollectionItemInput : IRequest<CollectionItemModelOutput>
    {
        public GetCollectionItemInput(Guid collectionId, Guid itemId)
        {
            CollectionId = collectionId;
            ItemId = itemId;
        }

        public Guid CollectionId { get; set; }
        public Guid ItemId { get; set; }
    }
}

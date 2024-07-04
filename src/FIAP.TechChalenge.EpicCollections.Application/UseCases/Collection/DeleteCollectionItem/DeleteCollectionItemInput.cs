using MediatR;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollectionItem
{
    public class DeleteCollectionItemInput : IRequest
    {
        public DeleteCollectionItemInput(Guid collectionId, Guid itemId)
        {
            CollectionId = collectionId;
            ItemId = itemId;
        }

        public Guid CollectionId { get; set; }
        public Guid ItemId { get; set; }
    }
}

using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollectionItem
{
    public interface IGetCollectionItem : IRequestHandler<GetCollectionItemInput, CollectionItemModelOutput>
    {
    }
}

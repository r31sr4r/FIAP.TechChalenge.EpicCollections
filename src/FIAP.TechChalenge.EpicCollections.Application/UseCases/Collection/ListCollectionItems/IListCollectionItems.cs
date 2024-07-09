using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.ListCollectionItems;
public interface IListCollectionItems : IRequestHandler<ListCollectionItemsInput, CollectionModelOutput>
{
}

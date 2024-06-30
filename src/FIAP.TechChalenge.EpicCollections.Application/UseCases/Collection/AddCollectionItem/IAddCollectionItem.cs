using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.AddCollectionItem;

public interface IAddCollectionItem : IRequestHandler<AddCollectionItemInput, CollectionItemModelOutput>
{
}

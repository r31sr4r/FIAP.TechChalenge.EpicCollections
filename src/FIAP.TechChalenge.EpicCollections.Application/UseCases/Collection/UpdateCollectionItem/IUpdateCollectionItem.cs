using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollectionItem;
public interface IUpdateCollectionItem
    : IRequestHandler<UpdateCollectionItemInput, CollectionItemModelOutput>
{ }

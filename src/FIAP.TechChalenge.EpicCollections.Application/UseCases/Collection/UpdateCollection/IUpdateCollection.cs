using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.UpdateCollection;
public interface IUpdateCollection
    : IRequestHandler<UpdateCollectionInput, CollectionModelOutput>
{ }

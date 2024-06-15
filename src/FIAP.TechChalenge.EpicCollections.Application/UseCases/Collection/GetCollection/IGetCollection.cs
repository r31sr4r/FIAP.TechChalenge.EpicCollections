using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollection;
public interface IGetCollection :
    IRequestHandler<GetCollectionInput, CollectionModelOutput>
{
}

using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.CreateCollection;
public interface ICreateCollection :
    IRequestHandler<CreateCollectionInput, CollectionModelOutput>
{
}

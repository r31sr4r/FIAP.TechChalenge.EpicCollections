using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.GetCollection;
public class GetCollectionInput : IRequest<CollectionModelOutput>
{
    public GetCollectionInput(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

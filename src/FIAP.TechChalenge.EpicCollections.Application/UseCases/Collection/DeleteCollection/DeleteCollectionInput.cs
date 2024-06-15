using MediatR;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollection;
public class DeleteCollectionInput : IRequest
{
    public DeleteCollectionInput(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

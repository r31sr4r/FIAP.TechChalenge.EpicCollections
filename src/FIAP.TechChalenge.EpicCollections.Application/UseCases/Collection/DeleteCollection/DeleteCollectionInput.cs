using FIAP.TechChalenge.EpicCollections.Domain.Entity;
using MediatR;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.Collection.DeleteCollection;
public class DeleteCollectionInput : IRequest
{
    public DeleteCollectionInput(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
    }

    public Guid Id { get; set; }
    public Guid UserId { get; set; }

}

using MediatR;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.User.DeleteUser;
public class DeleteUserInput : IRequest
{
    public DeleteUserInput(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

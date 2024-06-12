using MediatR;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.User.DeleteUser;
public interface IDeleteUser
    : IRequestHandler<DeleteUserInput>
{ }

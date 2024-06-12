using MediatR;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.User.ListUsers;
public interface IListUsers
    : IRequestHandler<ListUsersInput, ListUsersOutput>
{
}

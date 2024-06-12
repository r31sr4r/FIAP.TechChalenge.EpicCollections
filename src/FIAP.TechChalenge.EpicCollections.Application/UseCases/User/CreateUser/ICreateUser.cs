using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.User.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.User.CreateUser;
public interface ICreateUser :
    IRequestHandler<CreateUserInput, UserModelOutput>
{
}

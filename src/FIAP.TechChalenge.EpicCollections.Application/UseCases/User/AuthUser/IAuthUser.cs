using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.User.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.User.AuthUser;
public interface IAuthUser :
    IRequestHandler<AuthUserInput, UserModelOutput>
{
}

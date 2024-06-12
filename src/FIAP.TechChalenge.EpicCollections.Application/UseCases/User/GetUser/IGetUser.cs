using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.User.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.User.GetUser;
public interface IGetUser :
    IRequestHandler<GetUserInput, UserModelOutput>
{
}


using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.User.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.User.Update;
public interface IUpdateUser
    : IRequestHandler<UpdateUserInput, UserModelOutput>
{ }

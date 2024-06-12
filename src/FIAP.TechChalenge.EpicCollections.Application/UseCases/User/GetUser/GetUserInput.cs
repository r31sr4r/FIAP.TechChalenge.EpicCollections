using MediatR;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.User.Common;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.User.GetUser;
public class GetUserInput : IRequest<UserModelOutput>
{
    public GetUserInput(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

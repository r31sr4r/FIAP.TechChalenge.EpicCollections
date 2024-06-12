using FIAP.TechChalenge.EpicCollections.Application.UseCases.User.Common;

namespace FIAP.TechChalenge.EpicCollections.Api.ApiModels.User;

public class AuthResponse
{
    public string Email { get; set; }
    public string Token { get; set; }
}

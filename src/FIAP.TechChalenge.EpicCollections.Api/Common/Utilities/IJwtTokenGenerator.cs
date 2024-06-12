using FIAP.TechChalenge.EpicCollections.Application.UseCases.User.Common;

namespace FIAP.TechChalenge.EpicCollections.Api.Common.Utilities;

public interface IJwtTokenGenerator
{
    string GenerateJwtToken(UserModelOutput user);
}

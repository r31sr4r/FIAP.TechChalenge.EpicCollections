using FIAP.TechChalenge.EpicCollections.Application.Exceptions;
using FIAP.TechChalenge.EpicCollections.Application.UseCases.User.Common;
using FIAP.TechChalenge.EpicCollections.Domain.Common.Security;
using FIAP.TechChalenge.EpicCollections.Domain.Repository;

namespace FIAP.TechChalenge.EpicCollections.Application.UseCases.User.AuthUser;
public class AuthUser : IAuthUser
{
    private readonly IUserRepository _userRepository;

    public AuthUser(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserModelOutput> Handle(AuthUserInput request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmail(request.Email, cancellationToken);
        if (user == null)
            throw new CustomAuthenticationException("Invalid email or password.");

        if (!PasswordHasher.VerifyPasswordHash(request.Password, user.Password!))
            throw new CustomAuthenticationException("Invalid email or password.");

        return UserModelOutput.FromUser(user);
    }
}
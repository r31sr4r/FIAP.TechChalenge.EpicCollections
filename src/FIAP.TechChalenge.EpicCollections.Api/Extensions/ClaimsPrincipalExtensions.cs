using System.Security.Claims;

namespace FIAP.TechChalenge.EpicCollections.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "UID")?.Value;
        if (Guid.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }

        return null;
    }
}
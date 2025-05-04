using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Auth;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetSid(this ClaimsPrincipal claimsPrincipal)
    {
        var stringId = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Sid);

        if (stringId == null)
            throw new Exception($"Failed when reading logged in userId: {stringId}");

        return Guid.Parse(stringId);
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Auth;

public static class ClaimsPrincipalExtensions
{
    public static Guid ReadSid(this ClaimsPrincipal claimsPrincipal)
    {
        var sid = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Sid);

        if (sid == null)
        {
            throw new Exception($"Failed when reading logged-in user's SID: {sid}");
        }

        return Guid.Parse(sid);
    }
}
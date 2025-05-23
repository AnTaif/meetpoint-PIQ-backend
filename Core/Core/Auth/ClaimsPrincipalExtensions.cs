using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Auth;

public static class ClaimsPrincipalExtensions
{
    public static ContextUser ReadContextUser(this ClaimsPrincipal claimsPrincipal) =>
        new()
        {
            Id = claimsPrincipal.ReadSid(),
            Roles = claimsPrincipal.ReadRoles(),
        };

    public static Guid ReadSid(this ClaimsPrincipal claimsPrincipal)
    {
        var sid = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Sid);

        if (sid == null)
        {
            throw new Exception($"Failed when reading logged-in user's SID: {sid}");
        }

        return Guid.Parse(sid);
    }

    public static IReadOnlyCollection<string> ReadRoles(this ClaimsPrincipal claimsPrincipal)
    {
        var roles = claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToHashSet();
        return roles;
    }
}
using System.Security.Claims;

namespace BlogBackend.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserIdOrNull(this ClaimsPrincipal? user)
    {
        if (user is null)
        {
            return null;
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst("sub");
        if (userIdClaim is null)
        {
            return null;
        }

        return int.TryParse(userIdClaim.Value, out var userId) ? userId : null;
    }
}

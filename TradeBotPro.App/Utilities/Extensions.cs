using System.Security.Claims;

namespace TradeBotPro.App.Utilities;

public static class Extensions
{
    public static bool IsInRoles(this ClaimsPrincipal user, params string[] roles) => roles != null && roles.Any(x => user.IsInRole(x));
    public static string GetClaim(this ClaimsPrincipal user, string claimType) => user?.Claims?.FirstOrDefault(c => c.Type == claimType)?.Value;
}

using System.Security.Claims;

namespace MilestoneMotorsWebApp.Business.Utilities;

public static class ClaimsPrincipleExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}

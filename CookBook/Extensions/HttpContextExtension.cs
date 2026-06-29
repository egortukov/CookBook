using System.Security.Claims;

namespace CookBook.Extensions;

public static class HttpContextExtensions
{
    public static int? GetUserId(this HttpContext context)
    {
        var claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim is null)
        {
            return null;
        }

        return int.TryParse(claim.Value, out var id) ? id : null;
    }
}
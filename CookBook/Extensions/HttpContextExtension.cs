using System.Security.Claims;

namespace CookBook.Extensions;

public static class HttpContextExtensions
{
    public static int GetUserId(this HttpContext context)
    {
        var claim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        return int.Parse(claim!.Value);
    }
}
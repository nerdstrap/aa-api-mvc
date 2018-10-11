using System.Security.Claims;
using System.Security.Principal;

namespace Nerdstrap.Identity.IdentityManagerWeb.Security
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    return claim.Value;
                }
            }

            return null;
        }

        public static string GetAccessLevel(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var claim = claimsIdentity.FindFirst("accesslevel");
                if (claim != null)
                {
                    return claim.Value;
                }
            }

            return null;
        }
    }
}

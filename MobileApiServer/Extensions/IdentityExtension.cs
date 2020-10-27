using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

namespace MobileApiServer.Extensions
{
    public static class IdentityExtension
    {
        public static string LangId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("LangId");
            return claim != null ? claim.Value : string.Empty;
        }

        public static string Token(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Token");
            return claim != null ? claim.Value : string.Empty;
        }

        public static int SiteId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("SiteId");
            return int.TryParse(claim?.Value, out var i) ? i : 0;
        }

        public static int CustId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("CustId");
            return int.TryParse(claim?.Value, out var i) ? i : 0;
        }
    }

    /// <summary>
    /// seems don't need it if didn't save
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public int CustId { get; set; }
        public string LangId { get; set; }
        public int SiteId { get; set; }
        public string Token { get; set; }
    }
}

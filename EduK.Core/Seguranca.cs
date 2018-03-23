using System;
using System.Security.Claims;

namespace EduK.Core
{
    public static class Seguranca
    {

        public static string GetDisplayName(this ClaimsPrincipal principal)

        {
            var name = principal.Identity.Name;
            if (!String.IsNullOrEmpty(name)) return name;
            //var sub = principal.FindFirst(JwtClaimTypes.Subject);
            var sub = principal.FindFirst("sub");
            if (sub != null) return sub.Value;
            return "";
        }

        public static string GetUsuarioNome(this ClaimsPrincipal principal)
        {
            var name = principal.Identity.Name;
            if (!String.IsNullOrEmpty(name)) return name;
            //var sub = principal.FindFirst(JwtClaimTypes.Subject);
            var sub = principal.FindFirst("name");
            if (sub != null) return sub.Value;
            return "";
        }
    }
}

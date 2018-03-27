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
            var sub = principal.FindFirst("name");
            if (sub != null) return sub.Value;
            return "";
        }

        public static string GetUsuarioEmail(this ClaimsPrincipal principal)
        {
            var sub = principal.FindFirst("email");
            if (sub != null) return sub.Value;
            return "";
        }

        public static string GetUsuarioCargo(this ClaimsPrincipal principal)
        {
            var sub = principal.FindFirst("given_name");
            if (sub != null) return sub.Value;
            return "";
        }

        public static string GetUsuarioFoto(this ClaimsPrincipal principal)
        {
            var sub = principal.FindFirst("family_name");
            if (sub != null) return sub.Value;
            return "";
        }
    }
}

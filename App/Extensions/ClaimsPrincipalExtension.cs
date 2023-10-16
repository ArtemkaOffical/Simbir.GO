using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Simbir.GO.App.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return Guid.Parse(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
        }

        public static string GetUserRole(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
        }

        public static string GetUserLogin(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "UserName").Value;
        }
    }
}

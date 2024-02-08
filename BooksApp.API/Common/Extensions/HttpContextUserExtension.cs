using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PostsApp.Domain.Constants;

namespace PostsApp.Common.Extensions;

public static class HttpContextUserExtension
{
    public static string? GetUsername(this HttpContext httpContext)
    {
        return httpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
    }
    
    public static string? GetRole(this HttpContext httpContext)
    {
        return httpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
    }
    
    public static int? GetId(this HttpContext httpContext)
    {
        var id = httpContext.User.Claims.SingleOrDefault(claim => claim.Type == "id")?.Value;
        return id != null ? (int)Convert.ToInt32(id) : null;
    }

    public static void ChangeUsername(this HttpContext httpContext, string valueOfClaim)
    {
        var identity = httpContext.User.Identity as ClaimsIdentity;
        
        identity!.RemoveClaim(identity.FindFirst(ClaimTypes.NameIdentifier));
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, valueOfClaim));
    }

    public static async Task Login(this HttpContext httpContext, string username, int id, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim("id", Convert.ToString(id)),
            new Claim(ClaimTypes.Role, role)
        };
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity));
    }
}
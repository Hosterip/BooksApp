using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PostsApp.Common.Constants;

namespace PostsApp.Common.Extensions;

public static class HttpContextUserExtension
{
    public static string? GetEmail(this HttpContext httpContext)
    {
        return httpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
    }

    public static string? GetRole(this HttpContext httpContext)
    {
        return httpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
    }

    // You must be already sure that user is exists before using it 
    public static string? GetId(this HttpContext httpContext)
    {
        return httpContext.User.Claims.SingleOrDefault(claim => claim.Type == AdditionalClaimTypes.Id)?.Value;
    }

    public static string? GetSecurityStamp(this HttpContext httpContext)
    {
        return httpContext.User.Claims.SingleOrDefault(claim => claim.Type == AdditionalClaimTypes.SecurityStamp)
            ?.Value;
    }

    public static void ChangeEmail(this HttpContext httpContext, string valueOfClaim)
    {
        httpContext.ChangeClaim(ClaimTypes.Email, valueOfClaim);
    }

    public static void ChangeRole(this HttpContext httpContext, string valueOfClaim)
    {
        httpContext.ChangeClaim(ClaimTypes.Role, valueOfClaim);
    }

    public static void ChangeSecurityStamp(this HttpContext httpContext, string valueOfClaim)
    {
        httpContext.ChangeClaim(AdditionalClaimTypes.SecurityStamp, valueOfClaim);
    }

    public static async Task Login(this HttpContext httpContext, string id, string email, string role,
        string securityStamp)
    {
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true
        };
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role),
            new(AdditionalClaimTypes.SecurityStamp, securityStamp),
            new(AdditionalClaimTypes.Id, id)
        };
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    private static async void ChangeClaim(this HttpContext httpContext, string typeOfClaim, string valueOfClaim)
    {
        var claims = httpContext.User.Claims.ToList();
        var claim = claims.FirstOrDefault(c => c.Type == typeOfClaim);

        if (claim != null)
        {
            claims.Remove(claim);
            claims.Add(new Claim(typeOfClaim, valueOfClaim));
            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }
    }
}
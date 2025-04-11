using System.Security.Claims;
using BooksApp.API.Common.Constants;
using BooksApp.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BooksApp.API.Common.Services;

public class UserService(IHttpContextAccessor accessor) : IUserService
{
    private readonly HttpContext _httpContext = accessor.HttpContext!;
    
    public string? GetEmail()
    {
        return _httpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
    }

    public string? GetRole()
    {
        return _httpContext.User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
    }

    // You must be already sure that user is exists before using it 
    public Guid? GetId()
    {
        var claim = _httpContext.User.Claims.SingleOrDefault(claim => claim.Type == AdditionalClaimTypes.Id);
        if (Guid.TryParse(claim?.Value, out var userId))
            return userId;
        return null;
    }

    public string? GetSecurityStamp()
    {
        return _httpContext.User.Claims.SingleOrDefault(claim => claim.Type == AdditionalClaimTypes.SecurityStamp)
            ?.Value;
    }

    public void ChangeEmail(string valueOfClaim)
    {
        ChangeClaim(ClaimTypes.Email, valueOfClaim);
    }

    public void ChangeRole(string valueOfClaim)
    {
        ChangeClaim(ClaimTypes.Role, valueOfClaim);
    }

    public void ChangeSecurityStamp(string valueOfClaim)
    {
        ChangeClaim(AdditionalClaimTypes.SecurityStamp, valueOfClaim);
    }

    public async Task Login(string id, string email, string role, string securityStamp)
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
        await _httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    public async Task Logout()
    {
        await _httpContext.SignOutAsync();
    }

    private async void ChangeClaim(string typeOfClaim, string valueOfClaim)
    {
        var claims = _httpContext.User.Claims.ToList();
        var claim = claims.FirstOrDefault(c => c.Type == typeOfClaim);

        if (claim != null)
        {
            claims.Remove(claim);
            claims.Add(new Claim(typeOfClaim, valueOfClaim));
            var identity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await _httpContext.SignOutAsync();
            await _httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }
    }
}
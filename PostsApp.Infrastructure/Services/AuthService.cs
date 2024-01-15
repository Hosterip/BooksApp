using System.Security.Cryptography;
using PostsApp.Application.Interfaces;
using PostsApp.Domain.Auth;

namespace PostsApp.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;

    public AuthService(IUserService userService)
    {
        _userService = userService;
    }
    public async Task Register(string username, string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(32);
        string hash = AuthUtils.CreateHash(password, salt);
        await _userService.AddUser(username, hash, Convert.ToHexString(salt));
    }

    public void Login(string username, string password)
    {
        var user = _userService.GetSingleUserOrBadRequest(username, "User not found");
        string hash = AuthUtils.CreateHash(password, AuthUtils.StringToByteArray(user.Salt));
        if (hash != user.Hash) throw new AuthException("Password is incorrect");
    }
}
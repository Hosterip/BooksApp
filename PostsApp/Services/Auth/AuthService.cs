using System.Security.Cryptography;
using PostsApp.Requests.Auth;
using PostsApp.Services.Users;
using PostsApp.Shared.Utils;

namespace PostsApp.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;

    public AuthService(IUserService userService)
    {
        _userService = userService;
    }
    public async Task Register(AuthPostRequest request)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(32);
        string hash = AuthUtils.CreateHash(request.password, salt);
        
        await _userService.AddUser(request.username, hash, Convert.ToHexString(salt));
    }

    public void Login(AuthPostRequest request)
    {
        var user = _userService.GetSingleUserOrBadRequest(request.username, "User not found");
        string hash = AuthUtils.CreateHash(request.password, AuthUtils.StringToByteArray(user.Salt));
        if (hash != user.Hash) throw new BadHttpRequestException("Password is incorrect");
    }
}
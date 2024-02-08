namespace PostsApp.Contracts.Requests.Auth;

public class AuthUpdatePasswordRequest
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
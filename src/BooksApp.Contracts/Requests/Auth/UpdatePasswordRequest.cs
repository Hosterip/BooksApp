namespace PostsApp.Common.Contracts.Requests.Auth;

public class UpdatePasswordRequest
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
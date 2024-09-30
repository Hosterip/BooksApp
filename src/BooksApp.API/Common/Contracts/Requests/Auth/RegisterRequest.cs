namespace PostsApp.Common.Contracts.Requests.Auth;

public class RegisterRequest
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string Password { get; set; }
}

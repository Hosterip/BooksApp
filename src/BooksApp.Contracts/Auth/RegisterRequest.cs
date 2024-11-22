namespace BooksApp.Contracts.Auth;

public class RegisterRequest
{
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public string? MiddleName { get; init; }
    public string? LastName { get; init; }
    public required string Password { get; init; }
}
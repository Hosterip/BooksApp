namespace BooksApp.Contracts.Auth;

public class UpdatePasswordRequest
{
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
}
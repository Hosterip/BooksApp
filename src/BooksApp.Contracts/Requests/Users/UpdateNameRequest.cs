namespace BooksApp.Contracts.Requests.Users;

public class UpdateNameRequest
{
    public required string FirstName { get; init; }
    public required string? MiddleName { get; init; }
    public required string? LastName { get; init; }
}
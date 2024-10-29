namespace BooksApp.Contracts.Requests.Users;

public class UpdateNameRequest
{
    public required string FirstName { get; init; }
    public string? MiddleName { get; init; }
    public string? LastName { get; init; }
}
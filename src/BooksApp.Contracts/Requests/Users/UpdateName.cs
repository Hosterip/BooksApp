namespace BooksApp.Contracts.Requests.Users;

public class UpdateName
{
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
}
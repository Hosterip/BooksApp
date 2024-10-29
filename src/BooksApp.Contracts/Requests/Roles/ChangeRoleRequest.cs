namespace BooksApp.Contracts.Requests.Roles;

public class ChangeRoleRequest
{
    public Guid UserId { get; set; }
    public string Role { get; set; }
}
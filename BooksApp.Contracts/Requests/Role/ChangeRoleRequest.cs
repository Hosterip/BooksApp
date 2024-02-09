namespace PostsApp.Contracts.Requests.Role;

public class ChangeRoleRequest
{
    public int UserId { get; set; }
    public string Role { get; set; }
}
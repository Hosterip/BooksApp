namespace PostsApp.Common.Contracts.Requests.Role;

public class ChangeRoleRequest
{
    public int UserId { get; set; }
    public string Role { get; set; }
}
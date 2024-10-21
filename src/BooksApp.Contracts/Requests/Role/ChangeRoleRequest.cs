namespace PostsApp.Common.Contracts.Requests.Role;

public class ChangeRoleRequest
{
    public Guid UserId { get; set; }
    public string Role { get; set; }
}
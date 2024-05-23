namespace PostsApp.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public Role Role { get; set; }
    public string Hash { get; set; }
    public Image? Avatar { get; set; }
    public string Salt { get; set; }
}
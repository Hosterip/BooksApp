using System.ComponentModel.DataAnnotations;

namespace PostsApp.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public Role Role { get; set; }
    [Required]
    public string Hash { get; set; }
    [Required]
    public string Salt { get; set; }
}
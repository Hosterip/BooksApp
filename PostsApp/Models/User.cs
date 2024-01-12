using System.ComponentModel.DataAnnotations;

namespace PostsApp.Models;

public class User
{
    [Key]
    public string Username { get; set; }
    [Required]
    public string Hash { get; set; }
    [Required]
    public string Salt { get; set; }
}
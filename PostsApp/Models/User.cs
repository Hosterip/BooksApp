using System.ComponentModel.DataAnnotations;

namespace PostsApp.Models;

public class User
{
    [Key]
    [StringLength(255)]
    public string Username { get; set; }
    [Required]
    public string Hash { get; set; }
    [Required]
    public string Salt { get; set; }
}
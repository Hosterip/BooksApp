using System.ComponentModel.DataAnnotations;

namespace PostsApp.Domain.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [StringLength(255)]
    public string Username { get; set; }
    [Required]
    public string Hash { get; set; }
    [Required]
    public string Salt { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace PostsApp.Models;

public class Post
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(255)]
    public string Title { get; set; }
    [Required]
    [StringLength(255)]
    public string Body { get; set; }
    [Required]
    public User User { get; set; }
}
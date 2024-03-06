using System.ComponentModel.DataAnnotations;

namespace PostsApp.Domain.Models;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Body { get; set; }
    public User User { get; set; }
    public Book Book { get; set; }
}
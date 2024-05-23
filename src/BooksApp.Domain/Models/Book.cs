using System.ComponentModel.DataAnnotations;

namespace PostsApp.Domain.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Image Cover { get; set; }
    public User Author { get; set; }
}
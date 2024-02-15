namespace PostsApp.Domain.Models;

public class Like
{
    public int Id { get; set; }
    public User User { get; set; }
    public Book Book { get; set; }
}
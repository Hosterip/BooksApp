using PostsApp.Domain.Common.Models;
using PostsApp.Domain.Genre.ValueObjects;

namespace PostsApp.Domain.Genre;

public class Genre : AggregateRoot<GenreId>
{
    public string Name { get; set; }
    public List<Book.Book> Books { get; set; } = new List<Book.Book?>();
        
    private Genre(GenreId id) : base(id) { }
    public Genre(GenreId id, string name) : base(id)
    {
        Name = name;
    }
    
    public static Genre Create(string name)
    {
        return new(GenreId.CreateGenreId(), name);
    }
}
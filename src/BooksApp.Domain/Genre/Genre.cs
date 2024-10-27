using PostsApp.Domain.Common.Models;
using PostsApp.Domain.Genre.ValueObjects;

namespace PostsApp.Domain.Genre;

public class Genre : AggregateRoot<GenreId>
{
    private Genre(GenreId id) : base(id)
    {
    }

    private Genre(GenreId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; set; }
    public List<Book.Book> Books { get; set; } = new();

    public static Genre Create(string name)
    {
        return new Genre(GenreId.CreateGenreId(), name);
    }
}
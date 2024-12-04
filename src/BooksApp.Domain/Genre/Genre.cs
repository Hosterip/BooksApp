using BooksApp.Domain.Common.Models;
using BooksApp.Domain.Genre.ValueObjects;

namespace BooksApp.Domain.Genre;

public class Genre : AggregateRoot<GenreId>
{
    private Genre(GenreId id) : base(id)
    {
    }

    private Genre(GenreId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; init; }
    public IReadOnlyList<Book.Book> Books { get; init; }

    public static Genre Create(string name)
    {
        return new Genre(GenreId.CreateGenreId(), name);
    }
}
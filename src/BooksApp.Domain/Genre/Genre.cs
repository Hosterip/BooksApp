using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Constants.MaxLengths;
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
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name should be present and not whitespace");
        if (name.Length is > MaxPropertyLength.Genre.Name or < 1)
            throw new DomainException($"Name should be inclusively between 1 and {MaxPropertyLength.Genre.Name}");
            
        
        return new Genre(GenreId.CreateGenreId(), name);
    }
}
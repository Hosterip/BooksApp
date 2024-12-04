using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Models;
using BooksApp.Domain.Common.Utils;

namespace BooksApp.Domain.Book;

public class Book : AggregateRoot<BookId>
{
    private Book(BookId id) : base(id)
    {
    }

    private Book(BookId id, string title, string description, Image.Image cover, User.User author, List<Genre.Genre> genres) : base(id)
    {
        Title = title.TrimStart().TrimEnd();
        Description = description;
        Cover = cover;
        Author = author;
        ReferentialName = Title.GenerateRefName();
        _genres = genres;
    }

    private string _title { get; set; }

    public string Title
    {
        get => _title;
        set
        {
            ReferentialName = value.GenerateRefName();
            _title = value
                .TrimStart()
                .TrimEnd();
        }
    }

    public string Description { get; set; }
    public string ReferentialName { get; private set; }
    public Image.Image Cover { get; set; }
    public User.User Author { get; init; }
    private List<Genre.Genre> _genres;
    public IReadOnlyList<Genre.Genre> Genres => _genres;

    public static Book Create(string title, string description, Image.Image cover, User.User author, List<Genre.Genre> genres)
    {
        if (genres.Count == 0)
            throw new DomainException("Book must have at least one genre");
        
        return new Book(BookId.CreateBookId(), title, description, cover, author, genres);
    }

    public void ChangeGenres(List<Genre.Genre> genres)
    {
        if (genres.Count == 0)
            throw new DomainException("Book must have at least one genre");
        _genres = genres;
    }
}
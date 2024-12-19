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

    public string Title { get; private set; }

    public string Description { get; private set; }
    public string ReferentialName { get; private set; }
    public Image.Image Cover { get; private set; }
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

    public void ChangeTitle(string title)
    {
        ReferentialName = title.GenerateRefName();
        Title = title
            .TrimStart()
            .TrimEnd();
    }

    public void ChangeDescription(string description)
    {
        Description = description;
    }
}
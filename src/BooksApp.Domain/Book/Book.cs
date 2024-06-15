using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.Common.Models;
using PostsApp.Domain.Image.ValueObjects;
using PostsApp.Domain.Review.ValueObjects;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Domain.Book;

public class Book : AggregateRoot<BookId>
{
    private Book(BookId id) : base(id) { }
    private Book(BookId id, string title, string description, Image.Image cover, User.User author) : base(id)
    {
        Title = title;
        Description = description;
        Cover = cover;
        Author = author;
    }
    public string Title { get; set; }
    public string Description { get; set; }
    public Image.Image Cover { get; set; }
    public User.User Author { get; }
    public static Book Create(string title, string description, Image.Image cover, User.User author)
    {
        return new(BookId.CreateBookId(), title, description, cover, author);
    }
}
using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.Common.Models;
using PostsApp.Domain.Common.Utils;
using PostsApp.Domain.Image.ValueObjects;
using PostsApp.Domain.Review.ValueObjects;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Domain.Book;

public class Book : AggregateRoot<BookId>
{
    private Book(BookId id) : base(id) { }
    private Book(BookId id, string title, string description, Image.Image cover, User.User author) : base(id)
    {
        Title = title.TrimStart().TrimEnd();
        Description = description;
        Cover = cover;
        Author = author;
        ReferentialName = Title.GenerateRefName();
    }

    private string title { get; set; }
    public string Title
    {
        get => Title;
        set
        {
            ReferentialName = value.GenerateRefName();
            title = value 
                .TrimStart()
                .TrimEnd();
        } 
    }
    public string Description { get; set; }
    public string ReferentialName { get; private set; }
    public Image.Image Cover { get; set; }
    public User.User Author { get; }
    public List<Genre.Genre> Genres { get; set; }
    public static Book Create(string title, string description, Image.Image cover, User.User author)
    {
        return new(BookId.CreateBookId(), title, description, cover, author);
    }
}
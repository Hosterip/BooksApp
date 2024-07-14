using PostsApp.Domain.Bookshelf.Entities;
using PostsApp.Domain.Bookshelf.ValueObjects;
using PostsApp.Domain.Common.Models;

namespace PostsApp.Domain.Bookshelf;

public class Bookshelf : AggregateRoot<BookshelfId>
{
    public string Name { get; set; }
    public User.User User { get; set; }
    public ICollection<BookshelfBook> BookshelfBooks { get; set; } 
    private Bookshelf(BookshelfId id) : base(id) { }

    private Bookshelf(BookshelfId id, User.User user) : base(id)
    {
        User = user;
    }

    public static Bookshelf Create(User.User user)
    {
        return new(BookshelfId.CreateBookshelfId(), user);
    }

    public void Add(Book.Book book)
    {
        BookshelfBooks.Add(BookshelfBook.Create(book));
    }
}
using PostsApp.Domain.Bookshelf.Entities;
using PostsApp.Domain.Bookshelf.ValueObjects;
using PostsApp.Domain.Common.Models;
using PostsApp.Domain.Common.Utils;

namespace PostsApp.Domain.Bookshelf;

public class Bookshelf : AggregateRoot<BookshelfId>
{
    public string Name { get; set; }
    public string ReferentialName { get; set; }
    public User.User? User { get; set; }
    public List<BookshelfBook> BookshelfBooks { get; } 
    private Bookshelf(BookshelfId id) : base(id) { }

    private Bookshelf(BookshelfId id, User.User user, string name) : base(id)
    {
        User = user;
        BookshelfBooks = new List<BookshelfBook>();
        Name = name;
        ReferentialName = name.ConvertToReferencial();
    }

    public static Bookshelf Create(User.User user, string name)
    {
        return new(BookshelfId.CreateBookshelfId(), user, name);
    }

    public void AddBook(Book.Book book)
    {
        BookshelfBooks.Add(BookshelfBook.Create(book));
    }

    public void RemoveBook(Guid bookId)
    {
        var book = BookshelfBooks.FirstOrDefault();
        if(book != null) BookshelfBooks.Remove(book);
    }
}
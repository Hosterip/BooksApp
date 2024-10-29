using BooksApp.Domain.Bookshelf.Entities;
using BooksApp.Domain.Bookshelf.ValueObjects;
using BooksApp.Domain.Common.Models;
using BooksApp.Domain.Common.Utils;

namespace BooksApp.Domain.Bookshelf;

public class Bookshelf : AggregateRoot<BookshelfId>
{
    private string _name;

    private Bookshelf(BookshelfId id) : base(id)
    {
    }

    private Bookshelf(BookshelfId id, User.User user, string name) : base(id)
    {
        User = user;
        BookshelfBooks = new List<BookshelfBook>();
        Name = name;
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            ReferentialName = _name.GenerateRefName();
        }
    }

    public string ReferentialName { get; set; }
    public User.User? User { get; set; }
    public List<BookshelfBook> BookshelfBooks { get; }

    public static Bookshelf Create(User.User user, string name)
    {
        return new Bookshelf(BookshelfId.CreateBookshelfId(), user, name);
    }

    public void AddBook(Book.Book book)
    {
        BookshelfBooks.Add(BookshelfBook.Create(book));
    }

    public void RemoveBook(Guid bookId)
    {
        var book = BookshelfBooks.FirstOrDefault();
        if (book != null) BookshelfBooks.Remove(book);
    }
}
using BooksApp.Domain.Book.ValueObjects;
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
        _bookshelfBooks = new List<BookshelfBook>();
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

    public string ReferentialName { get; private set; }
    public User.User? User { get; init; }
    private readonly List<BookshelfBook> _bookshelfBooks = [];
    public IReadOnlyList<BookshelfBook> BookshelfBooks => _bookshelfBooks.AsReadOnly();

    public static Bookshelf Create(User.User user, string name)
    {
        return new Bookshelf(BookshelfId.CreateBookshelfId(), user, name);
    }

    public void AddBook(Book.Book book)
    {
        _bookshelfBooks.Add(BookshelfBook.Create(book));
    }

    public bool RemoveBook(Guid bookId)
    {
        var deletedCount = _bookshelfBooks.RemoveAll(book => book.Book.Id == BookId.CreateBookId(bookId));
        return deletedCount > 0;
    }
}
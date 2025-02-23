using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.Bookshelf.Entities;
using BooksApp.Domain.Bookshelf.ValueObjects;
using BooksApp.Domain.Common;
using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.Common.Helpers;
using BooksApp.Domain.Common.Models;
using BooksApp.Domain.User.ValueObjects;

namespace BooksApp.Domain.Bookshelf;

public class Bookshelf : AggregateRoot<BookshelfId>
{
    private Bookshelf(BookshelfId id) : base(id) { }

    private Bookshelf(
        BookshelfId id,
        User.User user,
        string name) : base(id)
    {
        UserId = user.Id;
        _bookshelfBooks = [];
        Name = name
            .TrimStart()
            .TrimEnd();
        ReferentialName = name.GenerateRefName();
    }

    public string Name { get; private set; }

    public string ReferentialName { get; private set; }
    public UserId UserId { get; init; }
    private readonly List<BookshelfBook> _bookshelfBooks = [];
    public IReadOnlyList<BookshelfBook> BookshelfBooks => _bookshelfBooks.AsReadOnly();

    public static Bookshelf Create(
        User.User user,
        string name)
    {
        ValidateName(name);
        
        return new Bookshelf(BookshelfId.Create(), user, name);
    }

    public void ChangeName(string name)
    {
        ValidateName(name);
        
        Name = name
            .TrimStart()
            .TrimEnd();
        ReferentialName = name.GenerateRefName();
    }
    
    public bool HasBook(Guid bookId)
    {
        return _bookshelfBooks.Any(x => x.Book.Id == BookId.Create(bookId));
    }

    public void AddBook(Book.Book book)
    {
        if (HasBook(book.Id.Value))
            throw new DomainException("Bookshelf already have this book");
        
        _bookshelfBooks.Add(BookshelfBook.Create(book));
    }

    public void RemoveBook(Guid bookId)
    {
        if (HasBook(bookId))
            throw new DomainException("Bookshelf does not have this book");
        
        _bookshelfBooks.RemoveAll(book => book.Book.Id == BookId.Create(bookId));
    }
    
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name could not be empty");

        if (name.Length is > MaxPropertyLength.Bookshelf.Name or < 1)
            throw new DomainException($"Name should be inclusively between 1 and {MaxPropertyLength.Bookshelf.Name}");
    }
}
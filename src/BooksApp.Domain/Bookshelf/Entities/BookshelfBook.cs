using PostsApp.Domain.Bookshelf.ValueObjects;
using PostsApp.Domain.Common.Models;

namespace PostsApp.Domain.Bookshelf.Entities;

public class BookshelfBook : Entity<BookshelfBookId>
{
    public Book.Book Book { get; set; }
    
    private BookshelfBook(BookshelfBookId id) : base(id) { }

    private BookshelfBook(BookshelfBookId id, Book.Book book) : base(id)
    {
        Book = book;
    }

    public static BookshelfBook Create(Book.Book book)
    {
        return new(BookshelfBookId.CreateBookshelfBookId(), book);
    }
}


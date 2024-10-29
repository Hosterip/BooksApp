using BooksApp.Domain.Bookshelf.ValueObjects;
using BooksApp.Domain.Common.Models;

namespace BooksApp.Domain.Bookshelf.Entities;

public class BookshelfBook : Entity<BookshelfBookId>
{
    private BookshelfBook(BookshelfBookId id) : base(id)
    {
    }

    private BookshelfBook(BookshelfBookId id, Book.Book book) : base(id)
    {
        Book = book;
    }

    public Book.Book Book { get; set; }

    public static BookshelfBook Create(Book.Book book)
    {
        return new BookshelfBook(BookshelfBookId.CreateBookshelfBookId(), book);
    }
}
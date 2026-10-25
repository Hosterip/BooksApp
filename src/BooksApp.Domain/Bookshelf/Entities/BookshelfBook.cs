using BooksApp.Domain.Common.Models;

namespace BooksApp.Domain;

public class BookshelfBook : Entity<BookshelfBookId>
{
    private BookshelfBook(BookshelfBookId id) : base(id)
    {
    }

    private BookshelfBook(BookshelfBookId id, Book book) : base(id)
    {
        Book = book;
    }

    public Book Book { get; set; }

    public static BookshelfBook Create(Book book)
    {
        return new BookshelfBook(BookshelfBookId.Create(), book);
    }
}
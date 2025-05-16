using Bogus;
using BooksApp.Domain.Book;
using BooksApp.Domain.Bookshelf;
using BooksApp.Domain.Bookshelf.Entities;
using BooksApp.Domain.User;
using TestCommon.Books;
using TestCommon.Users;

namespace TestCommon.Bookshelves;

public static class BookshelfFactory
{
    public static Bookshelf CreateBookshelf(
        User? user = null,
        string? name = null)
    {
        return new Faker<Bookshelf>()
            .CustomInstantiator(x => Bookshelf.Create(
                user: UserFactory.CreateUser(), 
                name: name ?? x.Lorem.Word()));
        
    }

    public static BookshelfBook CreateBookshelfBook(
        Book? book = null)
    {
        return BookshelfBook.Create(book ?? BookFactory.CreateBook());
    }
}
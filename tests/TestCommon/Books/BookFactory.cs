using Bogus;
using BooksApp.Domain.Book;
using BooksApp.Domain.Genre;
using BooksApp.Domain.Image;
using BooksApp.Domain.User;
using TestCommon.Genres;
using TestCommon.Images;
using TestCommon.Users;

namespace TestCommon.Books;

public static class BookFactory
{
    public static Book CreateBook(
        string? title = null,
        string? description = null,
        Image? cover = null,
        User? author = null,
        List<Genre>? genres = null)
    {
        return new Faker<Book>().CustomInstantiator(
            x => Book.Create(
                title ?? x.Lorem.Sentence(),
                description ?? x.Lorem.Paragraph(),
                cover ?? ImageFactory.CreateImage(),
                author ?? UserFactory.CreateUser(),
                genres ?? [GenreFactory.CreateGenre()]));
    }
}
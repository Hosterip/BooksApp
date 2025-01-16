using BooksApp.Domain.Book;
using BooksApp.Domain.Genre;
using BooksApp.Domain.Image;
using BooksApp.Domain.User;
using TestCommon.Common.Constants;
using TestCommon.Genres;
using TestCommon.Images;
using TestCommon.Users;

namespace TestCommon.Books;

public static class BookFactory
{
    public static Book CreateBook(
        string title = Constants.Books.Title,
        string description = Constants.Books.Description,
        Image? cover = null,
        User? author = null,
        List<Genre>? genres = null)
    {
        return Book.Create(
            title,
            description,
            cover ?? ImageFactory.CreateImage(),
            author ?? UserFactory.CreateUser(),
            genres ?? [GenreFactory.CreateGenre()]);
    }
}
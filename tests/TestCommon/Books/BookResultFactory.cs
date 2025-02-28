using BooksApp.Application.Books.Results;
using BooksApp.Application.Genres;
using BooksApp.Application.Users.Results;
using TestCommon.Common.Constants;
using TestCommon.Genres;
using TestCommon.Users;

namespace TestCommon.Books;

public static class BookResultFactory
{
    public static BookResult CreateBook(
        Guid? id = null,
        string title = Constants.Books.Title,
        string referentialName = Constants.Books.ReferentialName,
        string description = Constants.Books.Description,
        string? imageName = null,
        UserResult? author = null,
        List<GenreResult>? genres = null)
    {
        return new BookResult
        {
            Id = id.ToString() ?? Guid.NewGuid().ToString(),
            Title = title,
            ReferentialName = referentialName,
            Description = description,
            CoverName = imageName ?? Constants.Images.ImageName,
            Author = author ?? UserResultFactory.CreateUserResult(),
            Genres = genres ?? [GenreResultFactory.CreateGenreResult()],
            AverageRating = 0,
            Ratings = 0
        };
    }
}
using Bogus;
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
        string? title = null,
        string? referentialName = null,
        string? description = null,
        string? imageName = null,
        UserResult? author = null,
        List<GenreResult>? genres = null)
    {
        return new Faker<BookResult>()
            .RuleFor(x => x.Id, f => id.ToString() ?? f.Random.Guid().ToString())
            .RuleFor(x => x.Title, f => title ?? f.Lorem.Sentence())
            .RuleFor(x => x.ReferentialName, f => referentialName ?? f.Lorem.Slug())
            .RuleFor(x => x.Description, f => description ?? f.Lorem.Paragraph())
            .RuleFor(x => x.CoverName, f => imageName ?? f.Lorem.Slug())
            .RuleFor(x => x.Author, _ => author ?? UserResultFactory.CreateUserResult())
            .RuleFor(x => x.Genres, _ => genres ?? [GenreResultFactory.CreateGenreResult()])
            .RuleFor(x => x.AverageRating, f => 0)
            .RuleFor(x => x.Ratings, f => 0);
    }
}
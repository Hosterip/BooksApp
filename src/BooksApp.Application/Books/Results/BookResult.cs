using BooksApp.Application.Genres;
using BooksApp.Application.Users.Results;

namespace BooksApp.Application.Books.Results;

public class BookResult : RatingStatistics
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string ReferentialName { get; init; }
    public required string Description { get; init; }
    public required string CoverName { get; init; }
    public required UserResult Author { get; init; }
    public required IEnumerable<GenreResult> Genres { get; init; }
}
namespace BooksApp.Application.Genres;

public sealed class GenreResult
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
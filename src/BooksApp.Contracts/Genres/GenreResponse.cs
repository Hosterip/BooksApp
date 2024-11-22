namespace BooksApp.Contracts.Genres;

public class GenreResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
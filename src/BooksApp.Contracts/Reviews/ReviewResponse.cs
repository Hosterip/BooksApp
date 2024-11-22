using BooksApp.Contracts.Users;

namespace BooksApp.Contracts.Reviews;

public sealed class ReviewResponse
{
    public required Guid Id { get; init; }
    public required UserResponse User { get; init; }
    public required string Text { get; init; }
    public required int Rating { get; init; }
}
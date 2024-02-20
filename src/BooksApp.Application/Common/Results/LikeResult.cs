namespace PostsApp.Application.Common.Results;

public class LikeResult
{
    public required int Id { get; init; }
    public required int UserId { get; init; }
    public required int PostId { get; init; }
}
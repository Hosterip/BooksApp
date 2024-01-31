namespace PostsApp.Application.Common.Results;

public record PostWithoutUser
{
    public int Id { get; set; }
    public int LikeCount { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}
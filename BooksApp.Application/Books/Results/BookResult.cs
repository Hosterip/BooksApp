using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Books.Results;

public record BookResult
{
    public int Id { get; set; }
    public int LikeCount { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public UserResult Author { get; set; }
}
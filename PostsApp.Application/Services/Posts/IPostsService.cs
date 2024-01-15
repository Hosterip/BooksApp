using PostsApp.Contracts.Responses.Post;

namespace PostsApp.Application.Services.Posts;

public interface IPostsService
{
    public PostResult GetSinglePost(int id);
    public PostsResult GetPosts(int page, int limit, string query);
    public Task CreatePost(string title, string body, string username);
    public Task DeletePost(int id, string username);
}
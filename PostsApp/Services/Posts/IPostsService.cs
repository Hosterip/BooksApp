using PostsApp.Shared.Requests.Post;
using PostsApp.Shared.Responses.Post;

namespace PostsApp.Services.Posts;

public interface IPostsService
{
    public PostResponse GetSinglePost(int id);
    public PostsResponse GetPosts(int page, int limit, string query);
    public Task CreatePost(PostRequest postRequest, string username);
    public Task DeletePost(int id, string username);
}
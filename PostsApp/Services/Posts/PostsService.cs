using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PostsApp.DB;
using PostsApp.Models;
using PostsApp.Shared.Requests.Post;
using PostsApp.Shared.Responses.Post;
using PostsApp.Shared.Responses.User;

namespace PostsApp.Services.Posts;

public class PostsService : IPostsService
{
    private readonly AppDbContext _dbContext;

    public PostsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public PostResponse GetSinglePost(int id)
    {
        var post = _dbContext.Posts.Include(post => post.User).SingleOrDefault(post => post.Id == id);
        if(post == null) throw new BadHttpRequestException("Post not found");

        var user = new DefaultUserResponse { username = post.User.Username };
        
        return new PostResponse { id = post.Id, title = post.Title, body = post.Body, user = user };
    }

    public PostsResponse GetPosts(int page, int limit, string query)
    {
        var rawPosts =
            (from post in _dbContext.Posts
                where query.IsNullOrEmpty() || post.Title.Contains(query)
                let user = new DefaultUserResponse { username = post.User.Username }
                select new PostResponse {id = post.Id, title = post.Title, body = post.Body, user = user});
        var posts = rawPosts
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToArray();
        
        int totalCount = rawPosts.Count();
        int totalPages = (int)Math.Ceiling((decimal)totalCount / limit);

        return new PostsResponse { totalCount = totalCount, totalPages = totalPages, posts = posts };
    }

    public async Task CreatePost(PostRequest postRequest, string username)
    {
        var user = _dbContext.Users.SingleOrDefault(user => user.Username == username);
        if (user == null) throw new BadHttpRequestException("User not found");
        _dbContext.Posts.Add(new Post { User = user, Title = postRequest.title, Body = postRequest.body });
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeletePost(int id, string username)
    {
        var post = _dbContext.Posts.SingleOrDefault(post => post.Id == id && post.User.Username == username);
        if (post == null) throw new BadHttpRequestException("Post not found or post not yours");
        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync();
    }
}
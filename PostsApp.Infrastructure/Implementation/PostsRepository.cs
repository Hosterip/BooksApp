using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;
using PostsApp.Domain.Models;
using PostsApp.Infrastructure.Data;
using PostsApp.Infrastructure.DB;

namespace PostsApp.Infrastructure.Implementation;

public class PostsRepository : GenericRepository<Post>, IPostsRepository
{
    public PostsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedArray<PostResult>> GetPaginated(int limit, int page, string query) 
    {
        return await 
            (
                from post in _dbContext.Posts
                where query == null || post.Title.Contains(query)
                let user = new UserResult { Id = post.User.Id, Username = post.User.Username }
                join like in _dbContext.Likes.Include(like => like.Post)
                    on post.Id equals like.Post.Id into likes
                select new PostResult { Id = post.Id, Title = post.Title, Body = post.Body, User = user, LikeCount = likes.Count()})
            .PaginationAsync(page, limit);
    }

    public async Task<IEnumerable<PostResult>> GetPosts(Expression<Func<Post, bool>> expression)
    {
        return (
            from post in _dbContext.Posts
                .Where(expression)
                .Include(post => post.User)
            
            join like in _dbContext.Likes.Include(like => like.Post)
                on post.Id equals like.Post.Id into likes 
            let user = new UserResult{Id = post.User.Id, Username = post.User.Username }
            select new PostResult { Id = post.Id, Title = post.Title, Body = post.Body, LikeCount = likes.Count(), User = user }
        );
    }

    public override async Task<Post?> GetSingleWhereAsync(Expression<Func<Post, bool>> expression)
    {
        var post = await _dbContext.Posts
            .Include(post => post.User)
            .SingleAsync(expression);

        return post;
    }

    public override async Task<bool> AnyAsync(Expression<Func<Post, bool>> expression)
    {
        return await _dbContext.Posts
            .Include(post => post.User)
            .AnyAsync(expression);
    }
}
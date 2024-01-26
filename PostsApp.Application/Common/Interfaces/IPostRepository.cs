using System.Linq.Expressions;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Common.Interfaces;

public interface IPostRepository : IGenericRepository<Post>
{
    Task<PaginatedArray<PostResult>> GetPaginated(int limit, int page, string query);
    Task<PostResult> GetById(int id);
    Task<bool> PostAny(Expression<Func<Post, bool>> expression);
}
using PostsApp.Application.Common.Results;
using PostsApp.Application.Reviews.Results;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Common.Interfaces.Repositories;

public interface IReviewsRepository : IGenericRepository<Review>
{
    Task<PaginatedArray<ReviewResult>> GetPaginated(int bookId, int page, int limit);
}
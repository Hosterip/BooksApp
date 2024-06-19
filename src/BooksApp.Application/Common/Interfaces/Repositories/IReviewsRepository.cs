using PostsApp.Application.Common.Results;
using PostsApp.Application.Reviews.Results;
using PostsApp.Domain.Review;

namespace PostsApp.Application.Common.Interfaces.Repositories;

public interface IReviewsRepository : IGenericRepository<Review>
{
    Task<PaginatedArray<ReviewResult>> GetPaginated(Guid bookId, int page, int limit);
    Task<Review?> GetSingleById(Guid guid);
    Task<bool> AnyById(Guid guid);
}
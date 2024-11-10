using BooksApp.Application.Common.Results;
using BooksApp.Application.Reviews.Results;
using BooksApp.Domain.Review;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IReviewsRepository : IGenericRepository<Review>
{
    Task<PaginatedArray<ReviewResult>> GetPaginated(
        Guid? currentUserId,
        Guid bookId,
        int page,
        int limit);
    Task<Review?> GetSingleById(
        Guid reviewId,
        CancellationToken token = default);
    Task<bool> AnyById(
        Guid reviewId,
        CancellationToken token = default);
}
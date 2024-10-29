using BooksApp.Application.Common.Results;
using BooksApp.Application.Reviews.Results;
using BooksApp.Domain.Review;

namespace BooksApp.Application.Common.Interfaces.Repositories;

public interface IReviewsRepository : IGenericRepository<Review>
{
    Task<PaginatedArray<ReviewResult>> GetPaginated(Guid bookId, int page, int limit);
    Task<Review?> GetSingleById(Guid guid);
    Task<bool> AnyById(Guid guid);
}
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Reviews.Results;
using PostsApp.Domain.Models;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

public class ReviewsRepository : GenericRepository<Review>, IReviewsRepository 
{
    public ReviewsRepository(AppDbContext dbContext) : base(dbContext) { }
    
    public async Task<PaginatedArray<ReviewResult>> GetPaginated(int bookId, int page, int limit)
    {
        return await 
            (
                from review in _dbContext.Reviews
                    .Include(review => review.User)
                    .Include(review => review.User.Role)
                    .Include(review => review.Book)
                where review.Book.Id == bookId
                let user = new UserResult
                {
                    Id = review.User.Id,
                    Username = review.User.Username,
                    Role = review.User.Role.Name
                }
                select new ReviewResult
                {
                    Id = review.Id,
                    BookId = review.Book.Id,
                    User = user,
                    Rating = review.Rating,
                    Body = review.Body, 
                }
            )
            .PaginationAsync(page, limit);
    }
    
    public override async Task<bool> AnyAsync(Expression<Func<Review, bool>> expression)
    {
        return await _dbContext.Reviews
            .Include(review => review.User)
            .Include(review => review.User.Role)
            .Include(review => review.Book)
            .AnyAsync(expression);
    }

    public override async Task<Review?> GetSingleWhereAsync(Expression<Func<Review, bool>> expression)
    {
        return await _dbContext.Reviews
            .Include(review => review.User)
            .Include(review => review.User.Role)
            .Include(review => review.Book)
            .SingleOrDefaultAsync(expression);
    }
    public override async Task<IEnumerable<Review>> GetAllWhereAsync(Expression<Func<Review, bool>> expression)
    {
        return _dbContext.Reviews
            .Include(review => review.User)
            .Include(review => review.User.Role)
            .Include(review => review.Book)
            .Where(expression);
    }
}
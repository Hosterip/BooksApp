using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Domain.Models;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

public class ReviewsRepository : GenericRepository<Review>, IReviewsRepository 
{
    public ReviewsRepository(AppDbContext dbContext) : base(dbContext) { }
    
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
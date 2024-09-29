using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Reviews.Results;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.Review;
using PostsApp.Domain.Review.ValueObjects;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

public class ReviewsRepository : GenericRepository<Review>, IReviewsRepository 
{
    public ReviewsRepository(AppDbContext dbContext) : base(dbContext) { }
    
    public async Task<PaginatedArray<ReviewResult>> GetPaginated(Guid bookId, int page, int limit)
    {
        return await 
            (
                from review in _dbContext.Reviews
                where review.Book.Id == BookId.CreateBookId(bookId)
                let user = new UserResult
                {
                    Id = review.User.Id.Value.ToString(),
                    Email = review.User.Email,
                    FirstName = review.User.FirstName,
                    MiddleName = review.User.MiddleName,
                    LastName = review.User.LastName,
                    Role = review.User.Role.Name,
                    AvatarName = review.User.Avatar.ImageName
                }
                select new ReviewResult
                {
                    Id = review.Id.Value.ToString(),
                    BookId = review.Book.Id.Value.ToString(),
                    User = user,
                    Rating = review.Rating,
                    Body = review.Body, 
                }
            )
            .PaginationAsync(page, limit);
    }

    public async Task<Review?> GetSingleById(Guid guid)
    {
        return await _dbContext.Reviews.SingleOrDefaultAsync(review => review.Id == ReviewId.CreateReviewId(guid));
    }

    public async Task<bool> AnyById(Guid guid)
    {
        return await _dbContext.Reviews.AnyAsync(review => review.Id == ReviewId.CreateReviewId(guid));
    }
}
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
                where review.Book.Id == bookId
                let user = new UserResult
                {
                    Id = review.User.Id,
                    Username = review.User.Username,
                    Role = review.User.Role.Name,
                    AvatarName = review.User.Avatar.ImageName
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
}
using BooksApp.Application.Common.Extensions;
using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Reviews.Results;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.Review;
using BooksApp.Domain.Review.ValueObjects;
using BooksApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Infrastructure.Implementation.Repositories;

public class ReviewsRepository : GenericRepository<Review>, IReviewsRepository
{
    public ReviewsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedArray<ReviewResult>> GetPaginated(Guid? currentUserId, Guid bookId, int page, int limit)
    {
        return await
            (
                from review in _dbContext.Reviews
                    .Include(r => r.User.Followers)
                    .Include(r => r.User.Following)
                where review.Book.Id == BookId.CreateBookId(bookId)
                let viewerRelationship = review.User.ViewerRelationship(currentUserId)
                let user = new UserResult
                {
                    Id = review.User.Id.Value.ToString(),
                    Email = review.User.Email,
                    FirstName = review.User.FirstName,
                    MiddleName = review.User.MiddleName,
                    LastName = review.User.LastName,
                    Role = review.User.Role.Name,
                    AvatarName = review.User.Avatar.ImageName,
                    ViewerRelationship = new ViewerRelationship
                    {
                        IsFollowing = viewerRelationship.IsFollowing,
                        IsFriend = viewerRelationship.IsFriend,
                        IsMe = viewerRelationship.IsMe
                    }
                }
                select new ReviewResult
                {
                    Id = review.Id.Value.ToString(),
                    BookId = review.Book.Id.Value.ToString(),
                    User = user,
                    Rating = review.Rating,
                    Body = review.Body
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
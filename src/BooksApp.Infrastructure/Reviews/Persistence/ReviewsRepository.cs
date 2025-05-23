﻿using BooksApp.Application.Common.Extensions;
using BooksApp.Application.Common.Interfaces.Repositories;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Reviews.Results;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.Review;
using BooksApp.Domain.Review.ValueObjects;
using BooksApp.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Infrastructure.Reviews.Persistence;

public class ReviewsRepository : GenericRepository<Review>, IReviewsRepository
{
    public ReviewsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedArray<ReviewResult>> GetPaginated(Guid? currentUserId, Guid bookId, int page, int limit)
    {
        return await
            (
                from review in DbContext.Reviews
                    .Include(r => r.User.Followers)
                    .Include(r => r.User.Following)
                where review.Book.Id == BookId.Create(bookId)
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
                    ViewerRelationship = currentUserId == null
                        ? null
                        : new ViewerRelationship
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

    public async Task<Review?> GetSingleById(
        Guid reviewId,
        CancellationToken token = default)
    {
        return await DbContext.Reviews
            .SingleOrDefaultAsync(
                review => review.Id == ReviewId.Create(reviewId),
                token);
    }

    public async Task<bool> AnyById(
        Guid reviewId,
        CancellationToken token = default)
    {
        return await DbContext.Reviews.AnyAsync(
            review => review.Id == ReviewId.Create(reviewId),
            token);
    }
}
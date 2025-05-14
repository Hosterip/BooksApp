using BooksApp.Application.Reviews.Commands.CreateReview;
using BooksApp.Application.Reviews.Commands.DeleteReview;
using BooksApp.Application.Reviews.Commands.PrivilegedDeleteReview;
using BooksApp.Application.Reviews.Commands.UpdateReview;
using TestCommon.Common.Constants;

namespace TestCommon.Reviews;

public static class ReviewCommandFactory
{
    public static CreateReviewCommand CreateCreateReviewCommand(
        int rating = Constants.Reviews.Rating,
        string body = Constants.Reviews.Body,
        Guid? bookId = null)
    {
        return new CreateReviewCommand
        {
            Rating = rating,
            Body = body,
            BookId = bookId ?? Guid.NewGuid()
        };
    }

    public static UpdateReviewCommand CreateUpdateReviewCommand(
        int rating = Constants.Reviews.Rating,
        string body = Constants.Reviews.Body,
        Guid? reviewId = null)
    {
        return new UpdateReviewCommand
        {
            Rating = rating,
            Body = body,
            ReviewId = reviewId ?? Guid.NewGuid()
        };
    }

    public static DeleteReviewCommand CreateDeleteReviewCommand(
        Guid? reviewId = null)
    {
        return new DeleteReviewCommand
        {
            ReviewId = reviewId ?? Guid.NewGuid()
        };
    }

    public static PrivilegedDeleteReviewCommand CreatePrivilegedDeleteReviewCommand(
        Guid? reviewId = null)
    {
        return new PrivilegedDeleteReviewCommand
        {
            ReviewId = reviewId ?? Guid.NewGuid()
        };
    }
}
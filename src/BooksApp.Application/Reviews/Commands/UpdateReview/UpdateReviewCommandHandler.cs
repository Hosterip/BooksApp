using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Reviews.Results;

namespace PostsApp.Application.Reviews.Commands.UpdateReview;

internal sealed class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, ReviewResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ReviewResult> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _unitOfWork.Reviews.GetSingleWhereAsync(review => review.Id.Value == request.ReviewId);
        
        review!.Rating = request.Rating;
        review.Body = request.Body;

        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new ReviewResult
        {
            Id = review.Id.Value.ToString(),
            BookId = review.Book.Id.Value.ToString(),
            Body = review.Body,
            Rating = review.Rating,
            User = new UserResult
            {
                Id = review.User.Id.Value.ToString(),
                Username = review.User.Username,
                Role = review.User.Role.Name,
                AvatarName = review.User.Avatar?.ImageName
            }
        };
    }
}
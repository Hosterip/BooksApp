using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Reviews.Results;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Review;

namespace PostsApp.Application.Reviews.Commands.CreateReview;

internal sealed class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateReviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ReviewResult> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetSingleById(request.BookId);
        var user = await _unitOfWork.Users.GetSingleById(request.UserId);
        var review = Review.Create(request.Rating, request.Body, user!, book!);

        await _unitOfWork.Reviews.AddAsync(review);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new ReviewResult
        {
            Id = review.Id.Value.ToString(),
            BookId = book!.Id.Value.ToString(),
            Body = review.Body,
            Rating = review.Rating,
            User = new UserResult
            {
                Id = user!.Id.Value.ToString(),
                Username = user.Username,
                Role = user.Role.Name,
                AvatarName = user.Avatar?.ImageName
            }
        };
    }
}
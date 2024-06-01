using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Reviews.Results;
using PostsApp.Domain.Models;

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
        var book = await _unitOfWork.Books.GetSingleWhereAsync(book => book.Id == request.BookId);
        var user = await _unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.UserId);
        var review = new Review{User = user!, Rating = request.Rating, Book = book! , Body = request.Body};

        await _unitOfWork.Reviews.AddAsync(review);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new ReviewResult
        {
            Id = review.Id,
            BookId = book!.Id,
            Body = review.Body,
            Rating = review.Rating,
            User = new UserResult
            {
                Id = user!.Id,
                Username = user.Username,
                Role = user.Role.Name,
                AvatarName = user.Avatar?.ImageName
            }
        };
    }
}
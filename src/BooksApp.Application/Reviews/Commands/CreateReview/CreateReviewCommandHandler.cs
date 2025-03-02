using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Reviews.Results;
using BooksApp.Domain.Review;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Reviews.Commands.CreateReview;

internal sealed class CreateReviewCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IUserService userService)
    : IRequestHandler<CreateReviewCommand, ReviewResult>
{
    public async Task<ReviewResult> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var book = await unitOfWork.Books.GetSingleById(request.BookId, cancellationToken);
        var user = await unitOfWork.Users.GetSingleById(userService.GetId()!.Value, cancellationToken);
        var review = Review.Create(request.Rating, request.Body, user!, book!);

        await unitOfWork.Reviews.AddAsync(review, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);

        return mapper.Map<ReviewResult>(review);
    }
}
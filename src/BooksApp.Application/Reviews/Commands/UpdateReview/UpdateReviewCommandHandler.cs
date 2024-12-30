using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Reviews.Results;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Reviews.Commands.UpdateReview;

internal sealed class UpdateReviewCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<UpdateReviewCommand, ReviewResult>
{
    public async Task<ReviewResult> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await unitOfWork.Reviews.GetSingleById(request.ReviewId, cancellationToken);

        review!.ChangeRating(request.Rating);
        review.ChangeBody(request.Body);

        await unitOfWork.Reviews.Update(review);
        await unitOfWork.SaveAsync(cancellationToken);

        return mapper.Map<ReviewResult>(review);
    }
}
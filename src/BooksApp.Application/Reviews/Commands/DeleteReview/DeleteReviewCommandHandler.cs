using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Reviews.Commands.DeleteReview;

internal sealed class DeleteReviewCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteReviewCommand>
{
    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await unitOfWork.Reviews.GetSingleById(request.ReviewId, cancellationToken);

        await unitOfWork.Reviews.Remove(review!);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}
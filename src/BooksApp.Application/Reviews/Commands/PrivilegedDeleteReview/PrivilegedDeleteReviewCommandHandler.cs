using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Reviews.Commands.PrivilegedDeleteReview;

internal sealed class PrivilegedDeleteReviewCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<PrivilegedDeleteReviewCommand>
{
    public async Task Handle(PrivilegedDeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await unitOfWork.Reviews.GetSingleById(request.ReviewId, cancellationToken);

        await unitOfWork.Reviews.Remove(review!);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}
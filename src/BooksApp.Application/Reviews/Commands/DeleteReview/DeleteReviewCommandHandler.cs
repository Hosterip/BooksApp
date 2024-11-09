using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Reviews.Commands.DeleteReview;

internal sealed class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _unitOfWork.Reviews.GetSingleById(request.ReviewId, cancellationToken);

        _unitOfWork.Reviews.Remove(review!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Reviews.Commands.PrivilegedDeleteReview;

internal sealed class PrivilegedDeleteReviewCommandHandler : IRequestHandler<PrivilegedDeleteReviewCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public PrivilegedDeleteReviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PrivilegedDeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _unitOfWork.Reviews.GetSingleById(request.ReviewId);

        _unitOfWork.Reviews.Remove(review!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
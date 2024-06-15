using MediatR;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Reviews.Commands.DeleteReview;

internal sealed class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _unitOfWork.Reviews.GetSingleWhereAsync(review => review.Id.Value == request.ReviewId);
        
        _unitOfWork.Reviews.Remove(review!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
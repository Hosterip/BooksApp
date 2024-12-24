using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Reviews.Results;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Reviews.Commands.UpdateReview;

internal sealed class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, ReviewResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReviewResult> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _unitOfWork.Reviews.GetSingleById(request.ReviewId, cancellationToken);

        review!.Rating = request.Rating;
        review.Body = request.Body;

        await _unitOfWork.Reviews.Update(review);
        await _unitOfWork.SaveAsync(cancellationToken);

        return _mapper.Map<ReviewResult>(review);
    }
}
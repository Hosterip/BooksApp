using MapsterMapper;
using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Reviews.Results;

namespace PostsApp.Application.Reviews.Commands.UpdateReview;

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
        var review = await _unitOfWork.Reviews.GetSingleById(request.ReviewId);

        review!.Rating = request.Rating;
        review.Body = request.Body;

        await _unitOfWork.SaveAsync(cancellationToken);

        return _mapper.Map<ReviewResult>(review);
    }
}
using MapsterMapper;
using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Reviews.Results;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Reviews.Commands.UpdateReview;

internal sealed class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, ReviewResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

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
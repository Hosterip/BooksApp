using MapsterMapper;
using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Reviews.Results;
using PostsApp.Domain.Review;

namespace PostsApp.Application.Reviews.Commands.CreateReview;

internal sealed class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReviewResult> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetSingleById(request.BookId);
        var user = await _unitOfWork.Users.GetSingleById(request.UserId);
        var review = Review.Create(request.Rating, request.Body, user!, book!);

        await _unitOfWork.Reviews.AddAsync(review);
        await _unitOfWork.SaveAsync(cancellationToken);

        return _mapper.Map<ReviewResult>(review);
    }
}
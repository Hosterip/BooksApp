using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Reviews.Results;

namespace PostsApp.Application.Reviews.Queries.GetReviews;

public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, PaginatedArray<ReviewResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetReviewsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedArray<ReviewResult>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        var result = 
            await _unitOfWork.Reviews.GetPaginated(request.BookId, request.Page ?? 1, request.PageSize ?? 10);

        return result;
    }
}
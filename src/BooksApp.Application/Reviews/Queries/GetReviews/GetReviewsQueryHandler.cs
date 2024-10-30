using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Reviews.Results;
using MediatR;

namespace BooksApp.Application.Reviews.Queries.GetReviews;

internal sealed class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, PaginatedArray<ReviewResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetReviewsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedArray<ReviewResult>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        return
            await _unitOfWork.Reviews.GetPaginated(request.BookId, request.Page, request.Limit);
        ;
    }
}
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Reviews.Results;
using MediatR;

namespace BooksApp.Application.Reviews.Queries.GetReviews;

internal sealed class GetReviewsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetReviewsQuery, PaginatedArray<ReviewResult>>
{
    public async Task<PaginatedArray<ReviewResult>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        return
            await unitOfWork.Reviews.GetPaginated(request.CurrentUserId, request.BookId, request.Page, request.Limit);
        ;
    }
}
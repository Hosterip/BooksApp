using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Interfaces;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Books.Queries.GetSingleBook;

internal sealed class GetSingleBookQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<GetSingleBookQuery, BookResult>
{
    public async Task<BookResult> Handle(GetSingleBookQuery request, CancellationToken cancellationToken)
    {
        var book = await unitOfWork.Books.GetSingleById(request.Id, cancellationToken);

        var result = mapper.Map<BookResult>(book!);
        var bookStats = unitOfWork.Books.RatingStatistics(book!.Id.Value);
        result.AverageRating = bookStats.AverageRating;
        result.Ratings = bookStats.Ratings;

        return result;
    }
}
using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Common.Results;
using MediatR;

namespace BooksApp.Application.Books.Queries.GetBooks;

internal sealed class GetBooksQueryHandler(IUnitOfWork unitOfWork, IUserService userService)
    : IRequestHandler<GetBooksQuery, PaginatedArray<BookResult>>
{
    public async Task<PaginatedArray<BookResult>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var limit = request.Limit ?? 10;
        var page = request.Page ?? 1;

        var result = await unitOfWork.Books
            .GetPaginated(
                userService.GetId(),
                limit,
                page,
                request.Title,
                request.UserId,
                request.GenreId);

        return result;
    }
}
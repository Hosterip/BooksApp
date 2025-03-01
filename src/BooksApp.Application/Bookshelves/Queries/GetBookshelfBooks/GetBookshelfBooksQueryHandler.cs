using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Common.Results;
using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.GetBookshelfBooks;

internal sealed class GetBookshelfBooksQueryHandler(IUnitOfWork unitOfWork, IUserService userService)
    : IRequestHandler<GetBookshelfBooksQuery, PaginatedArray<BookResult>>
{
    public async Task<PaginatedArray<BookResult>> Handle(GetBookshelfBooksQuery request,
        CancellationToken cancellationToken)
    {
        var books = await unitOfWork.Books
            .GetPaginatedBookshelfBooks(
                userService.GetId()!.Value,
                request.BookshelfId, 
                request.Limit ?? 10,
                request.Page ?? 1)!;
        return books!;
    }
}
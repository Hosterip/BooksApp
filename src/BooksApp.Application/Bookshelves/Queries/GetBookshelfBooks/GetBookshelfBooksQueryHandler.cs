using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Common.Results;
using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.GetBookshelfBooks;

internal sealed class GetBookshelfBooksQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetBookshelfBooksQuery, PaginatedArray<BookResult>>
{
    public async Task<PaginatedArray<BookResult>> Handle(GetBookshelfBooksQuery request,
        CancellationToken cancellationToken)
    {
        var books = await unitOfWork.Books
            .GetPaginatedBookshelfBooks(request.CurrentUserId, request.BookshelfId, request.Limit ?? 10, request.Page ?? 1)!;
        return books!;
    }
}
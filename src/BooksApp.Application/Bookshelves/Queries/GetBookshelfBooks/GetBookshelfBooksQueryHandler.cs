using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Bookshelves.Queries.GetBookshelfBooks;

internal sealed class
    GetBookshelfBooksQueryHandler : IRequestHandler<GetBookshelfBooksQuery, PaginatedArray<BookResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBookshelfBooksQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedArray<BookResult>> Handle(GetBookshelfBooksQuery request,
        CancellationToken cancellationToken)
    {
        var books = await _unitOfWork.Books
            .GetPaginatedBookshelfBooks(request.BookshelfId, request.Limit ?? 10, request.Page ?? 1)!;
        return books!;
    }
}
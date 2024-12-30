using BooksApp.Application.Common.Interfaces;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.BookshelfById;

internal sealed class BookshelfByNameQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapster)
    : IRequestHandler<BookshelfByIdQuery, BookshelfResult>
{
    public async Task<BookshelfResult> Handle(BookshelfByIdQuery request, CancellationToken cancellationToken)
    {
        var bookshelf = await unitOfWork.Bookshelves.GetSingleById(
            request.BookshelfId, cancellationToken);
        var bookshelfResult = mapster.Map<BookshelfResult>(bookshelf!);
        return bookshelfResult;
    }
}
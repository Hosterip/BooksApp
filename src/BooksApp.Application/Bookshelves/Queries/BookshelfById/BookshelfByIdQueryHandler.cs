using BooksApp.Application.Common.Interfaces;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.BookshelfById;

internal sealed class BookshelfByNameQueryHandler : IRequestHandler<BookshelfByIdQuery, BookshelfResult>
{
    private readonly IMapper _mapster;
    private readonly IUnitOfWork _unitOfWork;

    public BookshelfByNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapster)
    {
        _unitOfWork = unitOfWork;
        _mapster = mapster;
    }

    public async Task<BookshelfResult> Handle(BookshelfByIdQuery request, CancellationToken cancellationToken)
    {
        var bookshelf = await _unitOfWork.Bookshelves.GetSingleById(
            request.BookshelfId, cancellationToken);
        var bookshelfResult = _mapster.Map<BookshelfResult>(bookshelf!);
        return bookshelfResult;
    }
}
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.User.ValueObjects;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Bookshelves.Queries.GetBookshelves;

internal sealed class GetBookshelvesQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<GetBookshelvesQuery, IEnumerable<BookshelfResult>>
{
    public async Task<IEnumerable<BookshelfResult>> Handle(GetBookshelvesQuery request, CancellationToken cancellationToken)
    {
        var rawBookshelves = await unitOfWork.Bookshelves
            .GetAllWhereAsync(bookshelf =>
                bookshelf.UserId == UserId.Create(request.UserId), cancellationToken);
        var bookshelves =
            rawBookshelves.Select(mapper.Map<BookshelfResult>).ToList();
        return bookshelves;
    }
}
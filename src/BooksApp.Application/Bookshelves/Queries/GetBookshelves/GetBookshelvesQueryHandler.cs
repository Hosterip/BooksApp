using MapsterMapper;
using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Application.Bookshelves.Queries.GetBookshelves;

internal sealed class GetBookshelvesQueryHandler : IRequestHandler<GetBookshelvesQuery, List<BookshelfResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBookshelvesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<BookshelfResult>> Handle(GetBookshelvesQuery request, CancellationToken cancellationToken)
    {
        var rawBookshelves = await _unitOfWork.Bookshelves
            .GetAllWhereAsync(bookshelf =>
                bookshelf.User != null && bookshelf.User.Id == UserId.CreateUserId(request.UserId));
        var bookshelves =
            rawBookshelves.Select(bookshelf => _mapper.Map<BookshelfResult>(bookshelf)).ToList();
        return bookshelves;
    }
}
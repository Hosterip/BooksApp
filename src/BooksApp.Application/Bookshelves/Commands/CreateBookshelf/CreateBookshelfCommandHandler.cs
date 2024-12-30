using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Bookshelf;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.CreateBookshelf;

internal sealed class CreateBookshelfCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<CreateBookshelfCommand, BookshelfResult>
{
    public async Task<BookshelfResult> Handle(CreateBookshelfCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetSingleById(request.UserId, cancellationToken);
        var bookshelf = Bookshelf.Create(user!, request.Name);
        await unitOfWork.Bookshelves.AddAsync(bookshelf, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);
        return mapper.Map<BookshelfResult>(bookshelf);
    }
}
using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBook;

internal sealed class RemoveBookCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveBookCommand>
{
    public async Task Handle(RemoveBookCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await unitOfWork.Bookshelves.GetSingleById(request.BookshelfId, cancellationToken);
        var removed = bookshelf!.RemoveBook(request.BookId);
        if (removed) await unitOfWork.SaveAsync(cancellationToken);
    }
}
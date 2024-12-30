using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.DeleteBookshelf;

internal sealed class DeleteBookshelfCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteBookshelfCommand>
{
    public async Task Handle(DeleteBookshelfCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await unitOfWork.Bookshelves.GetSingleById(request.BookshelfId, cancellationToken);
        
        await unitOfWork.Bookshelves.Remove(bookshelf!);
        
        await unitOfWork.SaveAsync(cancellationToken);
    }
}
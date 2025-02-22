using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.UpdateBookshelfName;

internal sealed class UpdateBookshelfNameCommandHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateBookshelfNameCommand>
{
    public async Task Handle(UpdateBookshelfNameCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await unitOfWork.Bookshelves
            .GetSingleById(request.BookshelfId, cancellationToken);
        bookshelf!.ChangeName(request.NewName);

        await unitOfWork.Bookshelves.Update(bookshelf);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}
using BooksApp.Application.Common.Interfaces;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.UpdateName;

internal sealed class UpdateNameCommandHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateNameCommand>
{
    public async Task Handle(UpdateNameCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await unitOfWork.Bookshelves
            .GetSingleById(request.BookshelfId, cancellationToken);
        bookshelf!.ChangeName(request.NewName);

        await unitOfWork.Bookshelves.Update(bookshelf);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}
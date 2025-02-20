using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Bookshelf;
using BooksApp.Domain.User.ValueObjects;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBookByName;

internal sealed class RemoveBookByNameCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveBookByNameCommand>
{
    public async Task Handle(RemoveBookByNameCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await unitOfWork.Bookshelves.GetSingleWhereAsync(bookshelf =>
            bookshelf.Name == request.BookshelfName &&
            bookshelf.UserId == UserId.Create(request.UserId), cancellationToken);
        if (bookshelf is null)
        {
            var user = await unitOfWork.Users.GetSingleById(request.UserId, cancellationToken);
            bookshelf = Bookshelf.Create(user!, request.BookshelfName);
            await unitOfWork.Bookshelves.AddAsync(bookshelf, cancellationToken);
        }

        bookshelf.RemoveBook(request.BookId);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}
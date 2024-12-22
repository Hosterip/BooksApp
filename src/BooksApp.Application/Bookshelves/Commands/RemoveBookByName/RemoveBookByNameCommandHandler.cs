using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Bookshelf;
using BooksApp.Domain.User.ValueObjects;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBookByName;

internal sealed class RemoveBookByNameCommandHandler : IRequestHandler<RemoveBookByNameCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveBookByNameCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveBookByNameCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await _unitOfWork.Bookshelves.GetSingleWhereAsync(bookshelf =>
            bookshelf.Name == request.BookshelfName &&
            bookshelf.UserId == UserId.CreateUserId(request.UserId), cancellationToken);
        if (bookshelf is null)
        {
            var user = await _unitOfWork.Users.GetSingleById(request.UserId, cancellationToken);
            bookshelf = Bookshelf.Create(user!, request.BookshelfName);
            await _unitOfWork.Bookshelves.AddAsync(bookshelf, cancellationToken);
        }

        var removed = bookshelf!.RemoveBook(request.BookId);
       if(removed) await _unitOfWork.SaveAsync(cancellationToken);
    }
}
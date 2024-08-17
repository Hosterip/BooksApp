using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Bookshelf;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Application.Bookshelves.Commands.RemoveBookFromDefaultBookshelf;

internal sealed class RemoveBookFromDefaultBookshelfCommandHandler : IRequestHandler<RemoveBookFromDefaultBookshelfCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveBookFromDefaultBookshelfCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveBookFromDefaultBookshelfCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await _unitOfWork.Bookshelves.GetSingleWhereAsync(bookshelf => 
            bookshelf.Name == request.BookshelfName &&
            bookshelf.User != null &&
            bookshelf.User.Id == UserId.CreateUserId(request.UserId));
        if (bookshelf is null)
        {
            var user = await _unitOfWork.Users.GetSingleById(request.UserId);
            bookshelf = Bookshelf.Create(user!, request.BookshelfName);
            await _unitOfWork.Bookshelves.AddAsync(bookshelf);
        }
        bookshelf!.RemoveBook(request.BookId);
    }
}
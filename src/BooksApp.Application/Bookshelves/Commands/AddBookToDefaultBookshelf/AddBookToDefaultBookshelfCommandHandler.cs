using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Bookshelf;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Application.Bookshelves.Commands.AddBookToDefaultBookshelf;

internal sealed class AddBookToDefaultBookshelfCommandHandler : IRequestHandler<AddBookToDefaultBookshelfCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddBookToDefaultBookshelfCommandHandler (IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddBookToDefaultBookshelfCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await _unitOfWork.Bookshelves.GetSingleWhereAsync(bookshelf => 
            bookshelf.Name == request.BookshelfName &&
            bookshelf.User != null &&
            bookshelf.User.Id == UserId.CreateUserId(request.UserId));
        if (bookshelf is null)
        {
            var user = await _unitOfWork.Users.GetSingleById(request.UserId);
            bookshelf = Bookshelf.Create(user!, request.BookshelfName.Trim().ToLowerInvariant());
            await _unitOfWork.Bookshelves.AddAsync(bookshelf);
        }
        var book = await _unitOfWork.Books.GetSingleById(request.BookId);
        bookshelf!.AddBook(book!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
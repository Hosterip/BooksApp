using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Bookshelf;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.AddBookByName;

internal sealed class AddBookByNameCommandHandler(IUnitOfWork unitOfWork, IUserService userService)
    : IRequestHandler<AddBookByNameCommand>
{
    public async Task Handle(AddBookByNameCommand request, CancellationToken cancellationToken)
    {
        var userId = userService.GetId()!.Value;

        var bookshelf =
            await unitOfWork.Bookshelves.GetBookshelfByName(request.BookshelfName, userId, cancellationToken);
        if (bookshelf is null)
        {
            var user = await unitOfWork.Users.GetSingleById(userId, cancellationToken);
            bookshelf = Bookshelf.Create(user!, request.BookshelfName);
            await unitOfWork.Bookshelves.AddAsync(bookshelf, cancellationToken);
        }

        var book = await unitOfWork.Books.GetSingleById(request.BookId, cancellationToken);
        bookshelf!.AddBook(book!);

        await unitOfWork.Bookshelves.Update(bookshelf);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}
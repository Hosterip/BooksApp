using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Bookshelf;
using BooksApp.Domain.User.ValueObjects;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.AddBookByName;

internal sealed class AddBookByNameCommandHandler : IRequestHandler<AddBookByNameCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddBookByNameCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddBookByNameCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await _unitOfWork.Bookshelves.GetSingleWhereAsync(bookshelf =>
            bookshelf.Name == request.BookshelfName &&
            bookshelf.User != null &&
            bookshelf.User.Id == UserId.CreateUserId(request.UserId), cancellationToken);
        if (bookshelf is null)
        {
            var user = await _unitOfWork.Users.GetSingleById(request.UserId, cancellationToken);
            bookshelf = Bookshelf.Create(user!, request.BookshelfName);
            await _unitOfWork.Bookshelves.AddAsync(bookshelf, cancellationToken);
        }

        var book = await _unitOfWork.Books.GetSingleById(request.BookId, cancellationToken);
        bookshelf!.AddBook(book!);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
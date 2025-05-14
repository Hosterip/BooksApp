using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Bookshelf;
using BooksApp.Domain.Common.Constants;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;

internal sealed class CreateDefaultBookshelvesCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateDefaultBookshelvesCommand>
{
    public async Task Handle(CreateDefaultBookshelvesCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetSingleById(request.UserId, cancellationToken);

        var read = Bookshelf.Create(user!, DefaultBookshelvesNames.Read);
        var currentlyReading = Bookshelf.Create(user!, DefaultBookshelvesNames.CurrentlyReading);
        var toRead = Bookshelf.Create(user!, DefaultBookshelvesNames.ToRead);

        await unitOfWork.Bookshelves.AddAsync(read, cancellationToken);
        await unitOfWork.Bookshelves.AddAsync(currentlyReading, cancellationToken);
        await unitOfWork.Bookshelves.AddAsync(toRead, cancellationToken);

        await unitOfWork.SaveAsync(cancellationToken);
    }
}
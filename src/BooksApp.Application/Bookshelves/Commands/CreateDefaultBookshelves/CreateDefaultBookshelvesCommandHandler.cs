using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Bookshelf;
using BooksApp.Domain.Common.Constants;
using MediatR;

namespace BooksApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;

internal sealed class CreateDefaultBookshelvesCommandHandler : IRequestHandler<CreateDefaultBookshelvesCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateDefaultBookshelvesCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateDefaultBookshelvesCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(request.UserId, cancellationToken);
        var read = Bookshelf.Create(user!, DefaultBookshelvesNames.Read);
        var currentlyReading = Bookshelf.Create(user!, DefaultBookshelvesNames.CurrentlyReading);
        var toRead = Bookshelf.Create(user!, DefaultBookshelvesNames.ToRead);
        await _unitOfWork.Bookshelves.AddAsync(read, cancellationToken);
        await _unitOfWork.Bookshelves.AddAsync(currentlyReading, cancellationToken);
        await _unitOfWork.Bookshelves.AddAsync(toRead, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Bookshelf;
using PostsApp.Domain.Common.Constants;

namespace PostsApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;

internal sealed class CreateDefaultBookshelvesCommandHandler : IRequestHandler<CreateDefaultBookshelvesCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateDefaultBookshelvesCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateDefaultBookshelvesCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(request.UserId);
        var read = Bookshelf.Create(user!, DefaultBookshelvesNames.Read);
        var currentlyReading = Bookshelf.Create(user!, DefaultBookshelvesNames.CurrentlyReading);
        var toRead = Bookshelf.Create(user!, DefaultBookshelvesNames.ToRead);
        await _unitOfWork.Bookshelves.AddAsync(read);
        await _unitOfWork.Bookshelves.AddAsync(currentlyReading);
        await _unitOfWork.Bookshelves.AddAsync(toRead);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}
using MapsterMapper;
using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Bookshelf;

namespace PostsApp.Application.Bookshelves.Commands.CreateBookshelf;

public class CreateBookshelfCommandHandler : IRequestHandler<CreateBookshelfCommand, BookshelfResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookshelfCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookshelfResult> Handle(CreateBookshelfCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(request.UserId);
        var bookshelf = Bookshelf.Create(user!, request.Name);
        await _unitOfWork.Bookshelves.AddAsync(bookshelf);
        await _unitOfWork.SaveAsync(cancellationToken);
        return _mapper.Map<BookshelfResult>(bookshelf);
    }
}
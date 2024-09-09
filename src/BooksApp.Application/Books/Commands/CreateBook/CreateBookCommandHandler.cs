using MapsterMapper;
using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Genres;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Book;
using PostsApp.Domain.Image;

namespace PostsApp.Application.Books.Commands.CreateBook;

internal sealed class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<BookResult> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(request.UserId);
        var image = Image.Create(request.ImageName);
        var book = Book.Create(request.Title, request.Description, image, user!);
        var genres = _unitOfWork.Genres.GetAllByIds(request.GenreIds);
        book.Genres = genres.ToList()!;
        await _unitOfWork.Images.AddAsync(image);
        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var result = _mapper.Map<BookResult>(book);
        result.AverageRating = 0;
        result.Ratings = 0;
        return result;
    }
}
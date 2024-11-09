using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Book;
using BooksApp.Domain.Image;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Books.Commands.CreateBook;

internal sealed class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookResult>
{
    private readonly IImageFileBuilder _imageFileBuilder;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IImageFileBuilder imageFileBuilder)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _imageFileBuilder = imageFileBuilder;
    }

    public async Task<BookResult> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(request.UserId, cancellationToken);

        // Images
        var imageName = await _imageFileBuilder.CreateImage(request.Image, cancellationToken);
        var image = Image.Create(imageName);
        // Book creation
        var book = Book.Create(request.Title, request.Description, image, user!);
        var genres = await _unitOfWork.Genres.GetAllByIds(request.GenreIds, cancellationToken);
        book.Genres = genres.ToList()!;
        await _unitOfWork.Images.AddAsync(image, cancellationToken);
        await _unitOfWork.Books.AddAsync(book, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var result = _mapper.Map<BookResult>(book);
        result.AverageRating = 0;
        result.Ratings = 0;
        return result;
    }
}
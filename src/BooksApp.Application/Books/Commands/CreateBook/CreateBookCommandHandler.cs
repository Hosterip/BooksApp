using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Book;
using BooksApp.Domain.Image;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Books.Commands.CreateBook;

internal sealed class CreateBookCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageFileBuilder imageFileBuilder)
    : IRequestHandler<CreateBookCommand, BookResult>
{
    public async Task<BookResult> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetSingleById(request.UserId, cancellationToken);

        // Images
        var imageName = await imageFileBuilder.CreateImage(request.Image, cancellationToken);
        var image = Image.Create(imageName);
        // Book creation
        var genres = await unitOfWork.Genres.GetAllByIds(request.GenreIds, cancellationToken);
        var book = Book.Create(request.Title, request.Description, image, user!, genres.ToList());
        await unitOfWork.Images.AddAsync(image, cancellationToken);
        await unitOfWork.Books.AddAsync(book, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);

        var result = mapper.Map<BookResult>(book);
        result.AverageRating = 0;
        result.Ratings = 0;
        return result;
    }
}
using MapsterMapper;
using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Genres;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Books.Commands.UpdateBook;

internal sealed class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookResult> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetSingleById(request.Id);
        if (request.Title is not null)
            book!.Title = request.Title;
        if (request.Description is not null)
            book!.Description = request.Description;
        if (request.ImageName is not null)
        {
            book!.Cover.ImageName = request.ImageName;
        }

        book!.Genres = _unitOfWork.Genres.GetAllByIds(request.GenreIds).ToList()!;

        await _unitOfWork.SaveAsync(cancellationToken);
        var result = _mapper.Map<BookResult>(book);
        var bookStats = _unitOfWork.Books.RatingStatistics(book.Id.Value);
        result.AverageRating = bookStats.AverageRating;
        result.Ratings = bookStats.Ratings;
        return result;
    }
}
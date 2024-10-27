using MapsterMapper;
using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Books.Queries.GetSingleBook;

internal sealed class GetSingleBookQueryHandler : IRequestHandler<GetSingleBookQuery, BookResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetSingleBookQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookResult> Handle(GetSingleBookQuery request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetSingleById(request.Id);

        var result = _mapper.Map<BookResult>(book!);
        var bookStats = _unitOfWork.Books.RatingStatistics(book!.Id.Value);
        result.AverageRating = bookStats.AverageRating;
        result.Ratings = bookStats.Ratings;

        return result;
    }
}
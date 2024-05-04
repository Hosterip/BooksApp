using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Books.Queries.GetBooks;

internal sealed class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, PaginatedArray<BookResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBooksQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<PaginatedArray<BookResult>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        string query = request.Query ?? "";
        int limit = request.Limit ?? 10;
        int page = request.Page ?? 1;

        PaginatedArray<BookResult> result;
        if (request.UserId == null)
        {
            result = await _unitOfWork.Books
                    .GetPaginated(limit, page, book => book.Title.Contains(query));
        }
        else
        {
            result = await _unitOfWork.Books
                .GetPaginated(limit, page, book => book.Title.Contains(query) && book.Author.Id == request.UserId);
        }
        
        
        return result;
    }
}
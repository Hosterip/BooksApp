using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using MediatR;

namespace BooksApp.Application.Users.Queries.GetUserFollowers;

internal sealed class GetUserFollowersQueryHandler : IRequestHandler<GetUserFollowersQuery, PaginatedArray<UserResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserFollowersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedArray<UserResult>> Handle(
        GetUserFollowersQuery request,
        CancellationToken cancellationToken)
    {
        var query = request.Query ?? "";
        var limit = request.Limit ?? 10;
        var page = request.Page ?? 1;

        var result = await _unitOfWork.Users.GetPaginatedFollowers(request.CurrentUserId, request.UserId, page, limit, query);
        return result;
    }
}
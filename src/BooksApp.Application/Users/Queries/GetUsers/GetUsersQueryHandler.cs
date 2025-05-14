using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using MediatR;

namespace BooksApp.Application.Users.Queries.GetUsers;

internal sealed class GetUsersQueryHandler(
    IUnitOfWork unitOfWork,
    IUserService userService)
    : IRequestHandler<GetUsersQuery, PaginatedArray<UserResult>>
{
    public async Task<PaginatedArray<UserResult>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var currentUserId = userService.GetId();

        var query = request.Query ?? "";
        var limit = request.Limit ?? 10;
        var page = request.Page ?? 1;

        var result = await unitOfWork.Users.GetPaginated(currentUserId, page, limit, query);
        return result;
    }
}
using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Users.Queries.GetUsers;

internal sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedArray<UserResult>>
{
    private readonly IAppDbContext _dbContext;

    public GetUsersQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedArray<UserResult>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        string? query = request.Query;
        int limit = request.Limit ?? 10;
        int page = request.Page;

        var rawUsers =
            from user in _dbContext.Users
            where query == null || user.Username.Contains(query)
            select new UserResult { username = user.Username };
        
        var result = await PaginatedArray<UserResult>.CreateAsync(rawUsers, page, limit);
        
        return result;
    }
}
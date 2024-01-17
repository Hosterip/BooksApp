using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Users.Queries.GetUsers;

internal class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, UsersResult>
{
    private readonly IAppDbContext _dbContext;

    public GetUsersQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UsersResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        string? query = request.Query;
        int limit = request.Limit ?? 10;
        int page = request.Page;
        
        var rawUsers =
            (from user in _dbContext.Users
                where query == null || user.Username.Contains(query)
                select new DefaultUserResult { username = user.Username });
        var users = rawUsers
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToArray();
        
        int totalCount = rawUsers.Count();
        int totalPages = (int)Math.Ceiling((decimal)totalCount / limit);
        return new UsersResult{users = users, totalPages = totalPages, totalCount = totalCount};
    }
}
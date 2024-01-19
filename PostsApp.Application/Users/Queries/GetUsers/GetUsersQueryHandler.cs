using MediatR;
using PostsApp.Application.Common.Extensions;
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

        var result = await 
            (
                from user in _dbContext.Users
                where query == null || user.Username.Contains(query)
                select new UserResult { Username = user.Username })
            .PaginationAsync(page, limit);
        
        return result;
    }
}
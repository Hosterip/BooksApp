using MediatR;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Posts.Results;

namespace PostsApp.Application.Posts.Queries.GetPosts;

internal sealed class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, PaginatedArray<PostResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPostsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<PaginatedArray<PostResult>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        string query = request.Query ?? "";
        int limit = request.Limit ?? 10;
        int page = request.Page ?? 1;

        var result = await _unitOfWork.Posts.GetPaginated(limit, page, query);
        
        return result;
    }
}
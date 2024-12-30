using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Users.Results;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Users.Queries.GetSingleUser;

internal sealed class GetSingleUserQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<GetSingleUserQuery, UserResult>
{
    public async Task<UserResult> Handle(GetSingleUserQuery request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetSingleById(
            request.Id, cancellationToken);

        return mapper.Map<UserResult>(user!);
    }
}
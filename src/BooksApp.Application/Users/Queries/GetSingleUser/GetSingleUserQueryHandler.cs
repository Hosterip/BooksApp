using MapsterMapper;
using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Users.Queries.GetSingleUser;

internal sealed class GetSingleUserQueryHandler : IRequestHandler<GetSingleUserQuery, UserResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetSingleUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserResult> Handle(GetSingleUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetSingleById(request.Id);

        return _mapper.Map<UserResult>(user!);
    }
}
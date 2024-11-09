using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Users.Results;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Users.Queries.GetSingleUser;

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
        var user = await _unitOfWork.Users.GetSingleById(
            request.Id, cancellationToken);

        return _mapper.Map<UserResult>(user!);
    }
}
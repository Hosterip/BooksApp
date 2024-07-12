using MapsterMapper;
using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Image;

namespace PostsApp.Application.Users.Commands.InsertAvatar;

public class InsertAvatarCommandHandler : IRequestHandler<InsertAvatarCommand, UserResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public InsertAvatarCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<UserResult> Handle(InsertAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = 
            await _unitOfWork.Users.GetSingleById(request.Id);
        if (user?.Avatar is null)
        {
            var image = Image.Create(request.ImageName);
            user!.Avatar = image;
            await _unitOfWork.Images.AddAsync(image);
        } else 
        {
            user.Avatar.ImageName = request.ImageName;
        }

        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<UserResult>(user);
    }
}
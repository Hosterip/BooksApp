using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Domain.Image;

namespace PostsApp.Application.Users.Commands.InsertAvatar;

public class InsertAvatarCommandHandler : IRequestHandler<InsertAvatarCommand, UserResult>
{
    private readonly IUnitOfWork _unitOfWork;
    public InsertAvatarCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<UserResult> Handle(InsertAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = 
            await _unitOfWork.Users.GetSingleWhereAsync(user => user.Id.Value == request.Id);
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
        
        return new UserResult
        {
            Id = user.Id.Value.ToString(),
            Username = user!.Username,
            Role = user.Role.Name,
            AvatarName = user.Avatar?.ImageName
        };
    }
}
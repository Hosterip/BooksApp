using BooksApp.Application.Common.Interfaces;
using BooksApp.Application.Users.Results;
using BooksApp.Domain.Image;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Users.Commands.InsertAvatar;

internal sealed class InsertAvatarCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageFileBuilder imageFileBuilder)
    : IRequestHandler<InsertAvatarCommand, UserResult>
{
    public async Task<UserResult> Handle(InsertAvatarCommand request, CancellationToken cancellationToken)
    {
        var user =
            await unitOfWork.Users.GetSingleById(request.Id, cancellationToken);
        var fileName = await imageFileBuilder.CreateImage(request.Image, cancellationToken);
        if (user?.Avatar is null)
        {
            var image = Image.Create(fileName!);
            user!.Avatar = image;
            await unitOfWork.Images.AddAsync(image, cancellationToken);
            
            await unitOfWork.Users.Update(user);
        }
        else
        {
            imageFileBuilder.DeleteImage(user.Avatar.ImageName);
            user.Avatar.ChangeImageName(fileName!);
            
            await unitOfWork.Images.Update(user.Avatar);
        }

        await unitOfWork.SaveAsync(cancellationToken);
        
        return mapper.Map<UserResult>(user);
    }
}
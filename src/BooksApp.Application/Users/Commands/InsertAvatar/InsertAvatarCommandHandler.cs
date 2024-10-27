using MapsterMapper;
using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Image;

namespace PostsApp.Application.Users.Commands.InsertAvatar;

public class InsertAvatarCommandHandler : IRequestHandler<InsertAvatarCommand, UserResult>
{
    private readonly IImageFileBuilder _imageFileBuilder;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public InsertAvatarCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IImageFileBuilder imageFileBuilder)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _imageFileBuilder = imageFileBuilder;
    }

    public async Task<UserResult> Handle(InsertAvatarCommand request, CancellationToken cancellationToken)
    {
        var user =
            await _unitOfWork.Users.GetSingleById(request.Id);
        if (user?.Avatar is null)
        {
            var fileName = await _imageFileBuilder.CreateImage(request.Image, cancellationToken);
            var image = Image.Create(fileName);
            user!.Avatar = image;
            await _unitOfWork.Images.AddAsync(image);
        }
        else
        {
            await _imageFileBuilder.DeleteImage(user.Avatar.ImageName, cancellationToken);
            var fileName = await _imageFileBuilder.CreateImage(request.Image, cancellationToken);
            user.Avatar.ImageName = fileName;
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return _mapper.Map<UserResult>(user);
    }
}
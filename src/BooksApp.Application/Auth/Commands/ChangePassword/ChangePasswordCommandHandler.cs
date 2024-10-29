using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Security;
using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Auth.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, AuthResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .GetSingleById(request.Id);

        if (Hashing.IsPasswordValid(user!.Hash, user.Salt, request.OldPassword))
            throw new ValidationException([
                new ValidationFailure(
                    nameof(ChangePasswordCommand.OldPassword),
                    AuthValidationMessages.Password)
            ]);

        var hashSalt = Hashing.GenerateHashSalt(request.NewPassword);
        user!.Hash = hashSalt.Hash;
        user.Salt = hashSalt.Salt;
        user.SecurityStamp = Guid.NewGuid().ToString();

        await _unitOfWork.SaveAsync(cancellationToken);
        return _mapper.Map<AuthResult>(user);
    }
}
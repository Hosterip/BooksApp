using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using MediatR;

namespace BooksApp.Application.Auth.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, AuthResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .GetSingleById(request.Id, cancellationToken);

        if (!user!.IsPasswordValid(_passwordHasher, request.OldPassword))
            throw new ValidationException([
                new ValidationFailure(
                    nameof(ChangePasswordCommand.OldPassword),
                    AuthValidationMessages.Password)
            ]);

        user.ChangePassword(_passwordHasher, request.NewPassword);
        
        user.SecurityStamp = Guid.NewGuid().ToString();

        await _unitOfWork.SaveAsync(cancellationToken);
        return _mapper.Map<AuthResult>(user);
    }
}
using MediatR;

namespace PostsApp.Application.Users.Commands.UpdateEmail;

public class UpdateEmailCommand : IRequest
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
}
using MediatR;
using SplitRight.Application.Commom;
using SplitRight.Application.DTOs;
using SplitRight.Domain.Contracts.Enums;

namespace SplitRight.Application.Features.Users.Commands
{
    public record RegisterUserCommand(string Name, string Email, string Password, UserRole Role) : IRequest<Result<UserDTO>>;
}

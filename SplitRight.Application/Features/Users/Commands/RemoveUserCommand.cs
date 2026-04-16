using MediatR;
using SplitRight.Application.Commom;
using SplitRight.Application.DTOs;

namespace SplitRight.Application.Features.Users.Commands
{
    public record RemoveUserCommand(Guid Id) : IRequest<Result<UserDTO>>;
}

using MediatR;
using SplitRight.Application.Commom;
using SplitRight.Application.DTOs;

namespace SplitRight.Application.Features.Users.Commands
{
    public record GetUserByIdCommand(Guid Id) : IRequest<Result<UserDTO>>;
}

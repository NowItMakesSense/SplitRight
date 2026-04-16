using MediatR;
using SplitRight.Application.Commom;
using SplitRight.Application.DTOs;

namespace SplitRight.Application.Features.Auth.Commands
{
    public record LoginUserCommand(string Email, string Password) : IRequest<Result<LoginDTO>>;
}

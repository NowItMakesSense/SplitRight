using MediatR;
using SplitRight.Application.Commom;
using SplitRight.Application.DTOs;

namespace SplitRight.Application.Features.Auth.Commands
{
    public record LogoutCommand(string RefreshToken) : IRequest<Result<LoginDTO>>;
}

using MediatR;
using SplitRight.Application.Commom;

namespace SplitRight.Application.Features.Auth.Commands
{
    public record RevokeRefreshTokenCommand(string RefreshToken) : IRequest<Result>;
}

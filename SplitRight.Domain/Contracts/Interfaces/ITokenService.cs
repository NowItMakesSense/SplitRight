using SplitRight.Domain.Contracts.Entities;

namespace SplitRight.Domain.Contracts.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user, Guid sessionId);
        string GenerateRefreshToken();
    }
}

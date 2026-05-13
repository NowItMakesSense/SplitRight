using SplitRight.Domain.Contracts.Entities;
using SplitRight.Domain.Contracts.Enums;

namespace SplitRight.Domain.Contracts.Interfaces
{
    public interface IJwtTokenGeneratorService
    {
        string GenerateAccessToken(User user, Guid sessionId);

        string GenerateRefreshToken();
    }
}

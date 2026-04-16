using SplitRight.Domain.Contracts.Enums;

namespace SplitRight.Domain.Contracts.Interfaces
{
    public interface IJwtTokenGeneratorService
    {
        string GenerateToken(Guid userId, string email, UserRole role);
    }
}

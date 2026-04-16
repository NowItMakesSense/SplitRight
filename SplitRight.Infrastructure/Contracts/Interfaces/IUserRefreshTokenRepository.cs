using SplitRight.Domain.Contracts.Entities;

namespace SplitRight.Infrastructure.Contracts.Interfaces
{
    public interface IUserRefreshTokenRepository
    {
        Task<UserRefreshToken?> GetByTokenAsync(
            string token,
            CancellationToken cancellationToken);

        Task AddAsync(
            UserRefreshToken refreshToken,
            CancellationToken cancellationToken);

        Task RevokeAllByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken);
    }
}

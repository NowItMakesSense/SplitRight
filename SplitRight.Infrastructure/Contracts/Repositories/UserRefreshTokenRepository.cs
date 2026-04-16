using Microsoft.EntityFrameworkCore;
using SplitRight.Domain.Contracts.Entities;
using SplitRight.Infrastructure.Contracts.Interfaces;
using SplitRight.Infrastructure.Contracts.Services;

namespace SplitRight.Infrastructure.Contracts.Repositories
{
    public class UserRefreshTokenRepository : IUserRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public UserRefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserRefreshToken?> GetByTokenAsync(
            string token,
            CancellationToken cancellationToken)
        {
            return await _context.UsersRefreshToken
                .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
        }

        public async Task AddAsync(
            UserRefreshToken refreshToken,
            CancellationToken cancellationToken)
        {
            await _context.UsersRefreshToken
                .AddAsync(refreshToken, cancellationToken);
        }

        public async Task RevokeAllByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var tokens = await _context.UsersRefreshToken
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToListAsync(cancellationToken);

            foreach (var token in tokens)
                token.Revoke();
        }
    }
}

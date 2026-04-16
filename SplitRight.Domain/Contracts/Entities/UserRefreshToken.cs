using SplitRight.Domain.Commom;

namespace SplitRight.Domain.Contracts.Entities
{
    public sealed class UserRefreshToken : BaseEntity
    {
        public Guid UserId { get; private set; }

        public string Token { get; private set; }

        public Guid SessionId { get; private set; }

        public DateTimeOffset ExpiresAt { get; private set; }

        public bool IsRevoked { get; private set; }

        public DateTimeOffset? RevokedAt { get; private set; }

        public string? ReplacedByToken { get; private set; }

        private UserRefreshToken() { }

        public UserRefreshToken(Guid userId, Guid sessionId, string token, DateTimeOffset expiresAt)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Token = token;
            SessionId = sessionId;
            ExpiresAt = expiresAt;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public bool IsActive() => !IsRevoked && DateTimeOffset.UtcNow <= ExpiresAt;

        public void Revoke(string? replacedByToken = null)
        {
            IsRevoked = true;
            RevokedAt = DateTimeOffset.UtcNow;
            ReplacedByToken = replacedByToken;
        }
    }
}

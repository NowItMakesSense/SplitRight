using Microsoft.AspNetCore.Identity;
using SplitRight.Domain.Contracts.Interfaces;

namespace SplitRight.Domain.Contracts.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<object> _hasher;

        public PasswordHasherService()
        {
            _hasher = new PasswordHasher<object>();
        }

        public string Hash(Object obj, string password)
        {
            return _hasher.HashPassword(obj, password);
        }

        public bool Verify(Object obj, string password, string hash)
        {
            var result = _hasher.VerifyHashedPassword(obj, hash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}

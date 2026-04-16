using Microsoft.AspNetCore.Http;
using SplitRight.Domain.Contracts.Enums;
using SplitRight.Domain.Contracts.Interfaces;
using SplitRight.Domain.Exceptions;
using System.Security.Claims;


namespace SplitRight.Domain.Contracts.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

        public Guid UserId
        {
            get
            {
                var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userId, out var id)) throw new OwnershipViolationException("Usuário não autenticado");

                return id;
            }
        }

        public string? Role => User?.FindFirst(ClaimTypes.Role)?.Value;

        public bool IsInRole(string role) => IsAuthenticated && User!.IsInRole(role);

        public bool IsAdmin => IsInRole(UserRole.Admin.ToString());
    }
}

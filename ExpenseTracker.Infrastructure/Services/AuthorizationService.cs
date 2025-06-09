using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Infrastructure.Services
{
    public interface IAuthorizationService
    {
        bool CanUserAccessResource(ClaimsPrincipal user, int resourceUserId, bool allowDefault = false);
        bool IsUserAdmin(ClaimsPrincipal user);
        int? GetCurrentUserId(ClaimsPrincipal user);
        string? GetCurrentUserRole(ClaimsPrincipal user);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly ILogger<AuthorizationService> _logger;

        public AuthorizationService(ILogger<AuthorizationService> logger)
        {
            _logger = logger;
        }

        public bool CanUserAccessResource(ClaimsPrincipal user, int resourceUserId, bool allowDefault = false)
        {
            var currentUserId = GetCurrentUserId(user);
            var currentUserRole = GetCurrentUserRole(user);

            if (currentUserRole == "Admin")
            {
                return true;
            }
            if (currentUserId.HasValue && currentUserId.Value == resourceUserId)
            {
                return true;
            }
            return allowDefault;
        }

        public bool IsUserAdmin(ClaimsPrincipal user)
        {
            return GetCurrentUserRole(user) == "Admin";
        }

        public int? GetCurrentUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            
            _logger.LogWarning("Could not parse user ID from claims: {UserIdClaim}", userIdClaim);
            return null;
        }

        public string? GetCurrentUserRole(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}

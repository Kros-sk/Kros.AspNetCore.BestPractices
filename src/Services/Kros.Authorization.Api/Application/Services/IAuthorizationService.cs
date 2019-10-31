using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Services
{
    /// <summary>
    /// Interface for user authorization service.
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Create jwt token from user claims. If user doesn't exist, this method will create it.
        /// </summary>
        /// <param name="organizationId">Organization ID.</param>
        /// <returns>Jwt token.</returns>
        Task<string> CreateJwtTokenAsync(long organizationId);
    }
}

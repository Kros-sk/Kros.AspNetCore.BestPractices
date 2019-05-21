using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Services
{
    public interface IAuthorizationService
    {
        Task<string> CreateJwtTokenAsync();
    }
}

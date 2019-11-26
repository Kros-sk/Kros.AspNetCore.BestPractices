using Kros.Organizations.Api.Properties;
using Kros.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kros.Organizations.Api.Application.Services
{
    /// <summary>
    /// Service providing methods to work with user roles.
    /// </summary>
    public class UserRoleService : IUserRoleService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserRoleOptions _userRoleOptions;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClientFactory">Http client factory (without name).</param>
        /// <param name="httpContextAccessor">Http context accessor.</param>
        /// <param name="userRoleOptions">Configuration options for user roles.</param>
        public UserRoleService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IOptions<UserRoleOptions> userRoleOptions)
        {
            _httpClientFactory = Check.NotNull(httpClientFactory, nameof(httpClientFactory));
            _httpContextAccessor = Check.NotNull(httpContextAccessor, nameof(httpContextAccessor));

            if (string.IsNullOrEmpty(userRoleOptions?.Value?.AuthServiceUrl))
            {
                throw new ArgumentException(Resources.StringNotNullOrEmpty, nameof(UserRoleOptions.AuthServiceUrl));
            }
            else
            {
                _userRoleOptions = userRoleOptions.Value;
            }
        }

        /// <inheritdoc />
        public async Task CreateOwnerRoleAsync(long userId, long organizationId)
        {
            // temporary solution, will be replaced with Service Bus
            string accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (accessToken != null && !string.IsNullOrEmpty(_userRoleOptions.AuthServiceUrl))
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    var userRoleControllerUrl = new Uri(_userRoleOptions.AuthServiceUrl);
                    var userRole = new { UserId = userId, OrganizationId = organizationId };

                    var httpContent = new StringContent(JsonSerializer.Serialize(userRole), Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", accessToken);
                    await client.PostAsync($"{userRoleControllerUrl}/Owner", httpContent);
                }
            }
        }

        /// <inheritdoc />
        public async Task DeleteUserRolesAsync(long organizationId)
        {
            string accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (accessToken != null)
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    var userRoleControllerUrl = new Uri(_userRoleOptions.AuthServiceUrl);

                    client.DefaultRequestHeaders.Add("Authorization", accessToken);
                    await client.DeleteAsync($"{userRoleControllerUrl}/{organizationId}");
                }
            }
        }
    }
}

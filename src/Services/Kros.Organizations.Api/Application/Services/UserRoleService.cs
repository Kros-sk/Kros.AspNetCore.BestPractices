﻿using Kros.AspNetCore.ServiceDiscovery;
using Kros.Utils;
using Microsoft.AspNetCore.Http;
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
        private readonly IServiceDiscoveryProvider _serviceDiscovery;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClientFactory">Http client factory (without name).</param>
        /// <param name="httpContextAccessor">Http context accessor.</param>
        /// <param name="serviceDiscovery"></param>
        public UserRoleService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IServiceDiscoveryProvider serviceDiscovery)
        {
            _httpClientFactory = Check.NotNull(httpClientFactory, nameof(httpClientFactory));
            _httpContextAccessor = Check.NotNull(httpContextAccessor, nameof(httpContextAccessor));
            _serviceDiscovery = Check.NotNull(serviceDiscovery, nameof(serviceDiscovery));
        }

        /// <inheritdoc />
        public async Task CreateOwnerRoleAsync(long userId, long organizationId)
        {
            string accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (accessToken != null)
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    var userRoleControllerUrl = GetPermissionsUri();
                    var userRole = new { UserId = userId, OrganizationId = organizationId };

                    var httpContent = new StringContent(JsonSerializer.Serialize(userRole), Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", accessToken);
                    await client.PostAsync($"{userRoleControllerUrl}/Owner", httpContent);
                }
            }
        }

        private Uri GetPermissionsUri() => _serviceDiscovery.GetPath("authorization", "permissions");

        /// <inheritdoc />
        public async Task DeleteUserRolesAsync(long organizationId)
        {
            string accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (accessToken != null)
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    var userRoleControllerUrl = GetPermissionsUri();

                    client.DefaultRequestHeaders.Add("Authorization", accessToken);
                    await client.DeleteAsync($"{userRoleControllerUrl}/{organizationId}");
                }
            }
        }
    }
}

﻿using System.Threading.Tasks;

namespace Kros.Organizations.Api.Application.Services
{
    public interface IUserRoleService
    {
        Task CreateOwnerRoleAsync(long userId, long companyId);
    }
}
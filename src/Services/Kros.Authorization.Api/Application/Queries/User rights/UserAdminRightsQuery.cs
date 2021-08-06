using MediatR;

namespace Kros.Authorization.Api.Application.Queries
{
    /// <summary>
    /// Query for determining if user has admin rights.
    /// </summary>
    public class UserAdminRightsQuery : IRequest<bool>
    {
    }
}

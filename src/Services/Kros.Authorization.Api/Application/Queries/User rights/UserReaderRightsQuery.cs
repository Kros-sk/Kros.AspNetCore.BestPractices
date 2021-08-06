using MediatR;

namespace Kros.Authorization.Api.Application.Queries
{
    /// <summary>
    /// Query for determining if user has reader rights.
    /// </summary>
    public class UserReaderRightsQuery : IRequest<bool>
    {
    }
}

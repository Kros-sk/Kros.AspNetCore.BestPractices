using MediatR;

namespace Kros.Authorization.Api.Application.Queries
{
    /// <summary>
    /// Query for determining if user has writer rights.
    /// </summary>
    public class UserWriterRightsQuery : IRequest<bool>
    {
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kros.Organizations.Api.Application.Commands.Pipelines
{
    /// <summary>
    /// Interface, which describe command for changing user resource.
    /// </summary>
    public interface IUserResourceCommand
    {
        /// <summary>
        /// Resource Id
        /// </summary>
        int Id { get; }

        /// <summary>
        /// User Id
        /// </summary>
        int UserId { get; }
    }
}

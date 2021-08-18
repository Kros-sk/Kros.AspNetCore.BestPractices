using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kros.Tags.Api.Application.Commands.Pipelines
{
    /// <summary>
    /// Interface which describe command for changing tag resource.
    /// </summary>
    public interface IIdCommand
    {
        /// <summary>
        /// Tag Id.
        /// </summary>
        long Id { get; }
    }
}

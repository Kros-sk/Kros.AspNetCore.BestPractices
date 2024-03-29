﻿using MediatR;
using Newtonsoft.Json;

namespace Kros.Tags.Api.Application.Commands
{
    /// <summary>
    /// Create tag command.
    /// </summary>
    public class CreateTagCommand : IRequest<long>
    {
        /// <summary>
        /// Tag name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Organization Id.
        /// </summary>
        [JsonIgnore]
        public long OrganizationId { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        [JsonIgnore]
        public long UserId { get; set; }

        /// <summary>
        /// ARGB value for color.
        /// </summary>
        public int ColorARGBValue { get; set; }
    }
}

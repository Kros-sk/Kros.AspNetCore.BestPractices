﻿using Kros.Organizations.Api.Application.Commands.Pipelines;
using MediatR;
using Newtonsoft.Json;

namespace Kros.Organizations.Api.Application.Commands
{
    /// <summary>
    /// Update Organization command.
    /// </summary>
    public class UpdateOrganizationCommand : IRequest, IUserResourceCommand
    {
        /// <summary>
        /// Id.
        /// </summary>
        [JsonIgnore]
        public long Id { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        [JsonIgnore]
        public long UserId { get; set; }

        /// <summary>
        /// Organization name
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Business Id
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// Address - Street
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Address - Street Number
        /// </summary>
        public string StreetNumber { get; set; }

        /// <summary>
        /// Address - City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Address - Zip Code
        /// </summary>
        public string ZipCode { get; set; }
    }
}
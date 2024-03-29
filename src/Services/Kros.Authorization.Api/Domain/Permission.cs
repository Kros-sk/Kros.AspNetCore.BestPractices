﻿using Kros.KORM.Metadata.Attribute;

namespace Kros.Authorization.Api.Domain
{
    /// <summary>
    /// User permission model.
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// User id.
        /// </summary>
        [Key("PK_Permissions", 1)]
        public int UserId { get; set; }

        /// <summary>
        /// Organization id.
        /// </summary>
        [Key("PK_Permissions", 2)]
        public int OrganizationId { get; set; }

        /// <summary>
        /// Permission key.
        /// </summary>
        [Key("PK_Permissions", 3)]
        public string Key { get; set; }

        /// <summary>
        /// Permission value.
        /// </summary>
        public string Value { get; set; }
    }
}
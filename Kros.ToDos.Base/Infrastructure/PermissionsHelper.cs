namespace Kros.ToDos.Base.Infrastructure
{
    /// <summary>
    /// Helper for permissions.
    /// </summary>
    public static class PermissionsHelper
    {
        /// <summary>
        /// Permission related HTTP request headers.
        /// </summary>
        public static class Headers
        {
            /// <summary>
            /// Organization ID.
            /// </summary>
            public const string OrganizationId = "Kros_OrganizationId";
        }

        /// <summary>
        /// Permission related claims.
        /// </summary>
        public static class Claims
        {
            /// <summary>
            /// Organization ID.
            /// </summary>
            public const string OrganizationId = "organization_id";
            /// <summary>
            /// Whether user is organization admin.
            /// </summary>
            public const string UserRole = "user_role";
        }

        /// <summary>
        /// Permission related claim values.
        /// </summary>
        public static class ClaimValues
        {
            /// <summary>
            /// User role value for organization Owner.
            /// </summary>
            public const string OwnerRole = "Owner";
            /// <summary>
            /// User role value for organization admin.
            /// </summary>
            public const string AdminRole = "Admin";
            /// <summary>
            /// User role value for organization writer.
            /// </summary>
            public const string WriterRole = "Write";
            /// <summary>
            /// User role value for organization reader.
            /// </summary>
            public const string ReaderRole = "Read";
        }
    }
}
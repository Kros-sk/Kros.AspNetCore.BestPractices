namespace Kros.Authorization.Api.Infrastructure
{
    public static class PermissionsHelper
    {
        public static class Headers
        {
            /// <summary>
            /// Name of HTTP request header containing organization ID.
            /// </summary>
            public const string OrganizationIdHeader = "Kros_OrganizationId";
        }

        public static class Claims
        {
            /// <summary>
            /// Organization ID.
            /// </summary>
            public const string OrganizationId = "organization_id";
        }
    }
}

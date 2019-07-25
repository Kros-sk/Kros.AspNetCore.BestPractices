namespace Kros.Organizations.Api.Infrastructure
{
    /// <summary>
    /// Helper for authorization.
    /// </summary>
    public static class PoliciesHelper
    {
        /// <summary>
        /// Authentication company admin policy name.
        /// </summary>
        public const string AdminAuthPolicyName = "Admin";

        /// <summary>
        /// Authentication company writer policy name.
        /// </summary>
        public const string WriterAuthPolicyName = "Write";

        /// <summary>
        /// Authentication company reader policy name.
        /// </summary>
        public const string ReaderAuthPolicyName = "Read";
    }
}
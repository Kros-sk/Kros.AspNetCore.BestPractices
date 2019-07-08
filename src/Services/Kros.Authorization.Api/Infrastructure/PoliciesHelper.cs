namespace Kros.ToDos.Api.Infrastructure
{
    /// <summary>
    /// Helper for authorization.
    /// </summary>
    public static class PoliciesHelper
    {
        /// <summary>
        /// Authentication company admin policy name.
        /// </summary>
        public const string AdminAuthPolicyName = "admin";

        /// <summary>
        /// Authentication company writer policy name.
        /// </summary>
        public const string WriterAuthPolicyName = "writer";

        /// <summary>
        /// Authentication company reader policy name.
        /// </summary>
        public const string ReaderAuthPolicyName = "reader";
    }
}
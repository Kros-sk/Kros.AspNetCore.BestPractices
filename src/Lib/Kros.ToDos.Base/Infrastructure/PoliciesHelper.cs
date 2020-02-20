namespace Kros.ToDos.Base.Infrastructure
{
    /// <summary>
    /// Helper for authorization.
    /// </summary>
    public static class PoliciesHelper
    {
        /// <summary>
        /// Authentication organization Owner policy name.
        /// </summary>
        public const string OwnerAuthPolicyName = "Owner";

        /// <summary>
        /// Authentication organization admin policy name.
        /// </summary>
        public const string AdminAuthPolicyName = "Admin";

        /// <summary>
        /// Authentication organization writer policy name.
        /// </summary>
        public const string WriterAuthPolicyName = "Write";

        /// <summary>
        /// Authentication organization reader policy name.
        /// </summary>
        public const string ReaderAuthPolicyName = "Read";
    }
}
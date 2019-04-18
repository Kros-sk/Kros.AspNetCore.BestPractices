using Kros.AspNetCore;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Configuration extensions.
    /// </summary>
    public static class IConfigurationExtensions
    {
        /// <summary>
        /// Get options from configuration.
        /// </summary>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <param name="configuration">Configuration.</param>
        /// <returns>Options.</returns>
        public static T GetSection<T>(
            this IConfiguration configuration) where T : class
            => configuration.GetSection(Helpers.GetSectionName<T>()).Get<T>();

        public static T GetSection<T>(
            this IConfiguration configuration, string sectioName) where T : class
            => configuration.GetSection(Helpers.GetSectionName<T>()).Get<T>();
    }
}

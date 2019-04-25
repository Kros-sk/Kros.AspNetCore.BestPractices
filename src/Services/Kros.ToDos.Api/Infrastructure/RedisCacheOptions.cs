namespace Kros.ToDos.Api.Infrastructure
{
    /// <summary>
    /// Redis cache options.
    /// </summary>
    public class RedisCacheOptions
    {
        /// <summary>
        /// Connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Instance name.
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        /// Use Redis cache? If <see langword="false"/> then memory distributed cache is used.
        /// </summary>
        public bool UseRedis { get; set; } = false;
    }
}

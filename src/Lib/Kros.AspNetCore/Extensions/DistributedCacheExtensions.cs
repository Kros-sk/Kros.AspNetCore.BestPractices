using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching.Distributed
{
    /// <summary>
    /// Extensions <see cref="IDistributedCache"/>.
    /// </summary>
    public static class DistributedCacheExtensions
    {
        /// <summary>
        /// Asynchronously sets <paramref name="value"/> in the specified cache with the specified <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="distributedCache">The cache in which to store the data.</param>
        /// <param name="key">The key to store the data in.</param>
        /// <param name="value">The data to store in the cache.</param>
        /// <param name="options">The cache options for the entry.</param>
        /// <param name="token">Optional. A System.Threading.CancellationToken to cancel the operation.</param>
        public async static Task SetAsync<T>(this IDistributedCache distributedCache,
            string key,
            T value,
            DistributedCacheEntryOptions options,
            CancellationToken token = default(CancellationToken))
           => await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), options, token);       

        /// <summary>
        /// Gets value from the specified cache with the specified <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache">The cache in which to store the data.</param>
        /// <param name="key">The key to get the stored data for.</param>
        /// <param name="token">Optional. A System.Threading.CancellationToken to cancel the operation.</param>
        /// <returns> A task that gets the value from the stored cache key.</returns>
        public async static Task<T> GetAsync<T>(
            this IDistributedCache distributedCache,
            string key,
            CancellationToken token = default(CancellationToken))
        {
            var result = await distributedCache.GetStringAsync(key, token);

            return String.IsNullOrWhiteSpace(result) ? default(T) : JsonConvert.DeserializeObject<T>(result);
        }
    }
}

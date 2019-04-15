﻿using Newtonsoft.Json;
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
            CancellationToken token = default)
           => await distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), options, token);

        /// <summary>
        /// Gets value from the specified cache with the specified <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="distributedCache">The cache in which to store the data.</param>
        /// <param name="key">The key to get the stored data for.</param>
        /// <param name="token">Optional. A System.Threading.CancellationToken to cancel the operation.</param>
        /// <returns> A task that gets the value from the stored cache key.</returns>
        public async static Task<T> GetAsync<T>(
            this IDistributedCache distributedCache,
            string key,
            CancellationToken token = default)
        {
            var result = await distributedCache.GetStringAsync(key, token);

            return String.IsNullOrWhiteSpace(result) ? default : JsonConvert.DeserializeObject<T>(result);
        }

        /// <summary>
        /// Gets value from specified cache with the specified <paramref name="key"/> 
        /// and if it don't exist use <paramref name="valueFactory"/> for to get it and cache it.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="distributedCache">The cache in which to store the data.</param>
        /// <param name="key">The key to get the stored data for.</param>
        /// <param name="valueFactory">Value factory for obtaining value if don't exist in cache.</param>
        /// <param name="options">The cache options for the entry.</param>
        /// <param name="token">Optional. A System.Threading.CancellationToken to cancel the operation.</param>
        /// <returns> A task that gets the value from the stored cache key.</returns>
        public async static Task<T> GetAndSetAsync<T>(
            this IDistributedCache distributedCache,
            string key,
            Func<T> valueFactory,
            DistributedCacheEntryOptions options,
            CancellationToken token = default)
        {
            var result = await distributedCache.GetAsync<T>(key, token);

            if (result == null)
            {
                result = valueFactory();
                await distributedCache.SetAsync(key, result, options, token);
            }

            return result;
        }
    }    
}

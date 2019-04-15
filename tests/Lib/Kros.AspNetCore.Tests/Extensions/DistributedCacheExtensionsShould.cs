﻿using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Kros.AspNetCore.Tests.Extensions
{
    public class DistributedCacheExtensionsShould
    {
        #region Test class

        class Foo
        {
            public int Value { get; set; }
        }

        #endregion

        [Fact]
        public async Task SetValueToCache()
        {
            IDistributedCache cache = new MemoryDistributedCache();

            await cache.SetAsync("2", 2, new DistributedCacheEntryOptions());

            var value = await cache.GetAsync<int>("2");

            value.Should().Be(2);
        }

        [Fact]
        public async Task SetComplexValueToCache()
        {
            IDistributedCache cache = new MemoryDistributedCache();

            await cache.SetAsync("foo", new Foo() { Value = 2 }, new DistributedCacheEntryOptions());

            var value = await cache.GetAsync<Foo>("foo");

            value.Value.Should().Be(2);
        }

        [Fact]
        public async Task GetDefaultWhenKeyDoesntExist()
        {
            IDistributedCache cache = new MemoryDistributedCache();

            var foo = await cache.GetAsync<Foo>("foo");
            foo.Should().BeNull();

            var @int = await cache.GetAsync<int?>("int");
            @int.Should().BeNull();

            var @string = await cache.GetAsync<string>("string");
            @string.Should().BeNull();
        }

        internal class MemoryDistributedCache : IDistributedCache
        {
            private Dictionary<string, byte[]> _cache = new Dictionary<string, byte[]>();

            public byte[] Get(string key)
                => _cache.TryGetValue(key, out var value) ? value : null;

            public Task<byte[]> GetAsync(string key, CancellationToken token = default)
                => Task.FromResult(Get(key));

            public void Refresh(string key)
            {
                throw new NotImplementedException();
            }

            public Task RefreshAsync(string key, CancellationToken token = default)
            {
                throw new NotImplementedException();
            }

            public void Remove(string key)
            {
                throw new NotImplementedException();
            }

            public Task RemoveAsync(string key, CancellationToken token = default)
            {
                throw new NotImplementedException();
            }

            public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
                => _cache.TryAdd(key, value);

            public Task SetAsync(string key,
                byte[] value,
                DistributedCacheEntryOptions options,
                CancellationToken token = default)
            {
                Set(key, value, options);

                return Task.CompletedTask;
            }
        }
    }
}

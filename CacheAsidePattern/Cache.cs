using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace CacheAsidePattern
{
    public interface ICache
    {
        int ItemExperationInSeconds { get; set; }

        bool TryGet<T>(int key, out T item) where T : class;
        void Put(int key, object item);
    }


    public class Cache : ICache
    {
        ObjectCache _memoryCache = new MemoryCache("PageUpCache");

        public Cache()
        {
            ItemExperationInSeconds = 3600; //hour
        }

        
        public int ItemExperationInSeconds { get; set; }

        public bool TryGet<T>(int key, out T item) where T : class
        {
            item = null;

            CacheItem cacheItem = _memoryCache.GetCacheItem(key.ToString());

            if (cacheItem == null) return false;

            item = cacheItem.Value as T;

            return true;
        }

        public void Put(int key, object item)
        {
            DateTimeOffset expire = new DateTimeOffset(DateTime.UtcNow).AddSeconds(ItemExperationInSeconds);

            _memoryCache.Add(key.ToString(), item, expire);
        }
    }
}

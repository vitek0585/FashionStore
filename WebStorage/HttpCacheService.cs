using System;
using System.Threading;
using System.Web;
using System.Web.Caching;
using WebCache.Interfaces;

namespace WebCache
{
    public class HttpCacheConfig : IHttpCacheConfig
    {

        public DateTime AbsoluteExpiration { get; set; }
        public TimeSpan SlidingExpiration { get; set; }
        public CacheDependency Dependency { get; set; }
        public CacheItemPriority Priority { get; set; }
        public CacheItemRemovedCallback Callback { get; set; }

        public HttpCacheConfig()
        {
            Priority = CacheItemPriority.Default;
        }


    }
    public class HttpCacheService : IHttpCache
    {
        private Cache _cache;
        private static object _sync = new object();
        public HttpCacheService()
        {
            _cache = HttpContext.Current.Cache;

        }
        public HttpCacheService(Cache cache)
        {
            _cache = cache;

        }
        public TResult TryGetValue<TResult>(string key, Func<TResult> value, IHttpCacheConfig config = null)
        {
            if (_cache[key] != null)
            {
                return (TResult)_cache[key];
            }
            HttpCacheConfig manual = config as HttpCacheConfig;

            if (manual != null)
            {
                bool isInit = false;
                TResult res = default(TResult);
               
                LazyInitializer.EnsureInitialized(ref res, ref isInit, ref _sync, () => value.Invoke());
          
                _cache.Insert(key, res, manual.Dependency, manual.AbsoluteExpiration,
                    manual.SlidingExpiration, manual.Priority, manual.Callback);
            }

            return (TResult)_cache[key];
        }

    }
}
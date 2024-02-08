using CruZ.Editor.Controls;
using System;
using System.Collections.Generic;
using System.IO;

namespace CruZ.Editor
{
    class CacheService
    {
        public const string CACHE_ROOT = "caches";

        public static void Register(ICacheControl control)
        {
            _cacheControls.Add(control);

            control.UpdateCache -= Control_UpdateCache;
            control.UpdateCache += Control_UpdateCache;
        }

        private static void Control_UpdateCache(ICacheControl cache)
        {
            ReadCache(cache);
        }

        public static void CallReadCaches()
        {
            foreach (var cache in _cacheControls)
            {
                ReadCache(cache);
            }
        }

        public static void CallWriteCaches()
        {
            foreach (var cache in _cacheControls)
            {
                WriteCache(cache);
            }
        }

        private static void ReadCache(ICacheControl cacheControl)
        {
            var cachePath = GetCachePath(cacheControl);

            if(!File.Exists(cachePath))
            {
                return;
            }

            bool cacheRead = false;
            try
            {
                using var file = File.OpenRead(cachePath);
                cacheRead = cacheControl.ReadCache(file);
            }
            catch
            {
                if(!cacheRead) throw;
            }
        }

        private static void WriteCache(ICacheControl cacheControl)
        {
            var cachePath = GetCachePath(cacheControl);
            using var file = File.OpenWrite(cachePath);
            cacheControl.WriteCache(file);
        }

        public static string GetCachePath(ICacheControl cachedControl)
        {
            return Path.Combine(CACHE_ROOT, cachedControl.UniquedCachedPath);
        }

        private static HashSet<ICacheControl> _cacheControls = [];
    }
}
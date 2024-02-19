using CruZ.Editor.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CruZ.Editor
{
    class CacheService
    {
        public const string CACHE_ROOT = "caches";

        public static void Register(INeedCache control)
        {
            _cacheControls.Add(control);

            control.CacheRead -= Control_CacheRead;
            control.CacheRead += Control_CacheRead;

            control.CacheWrite -= Control_CacheWrite;    
            control.CacheWrite += Control_CacheWrite;    
        }

        private static void Control_CacheWrite(INeedCache cache, string key)
        {
            WriteCache(cache, key);
        }

        private static void Control_CacheRead(INeedCache cache, string key)
        {
            ReadCache(cache, key);
        }

        //public static void CallReadCaches()
        //{
        //    foreach (var cache in _cacheControls)
        //    {
        //        ReadCache(cache);
        //    }
        //}

        //public static void CallWriteCaches()
        //{
        //    foreach (var cache in _cacheControls)
        //    {
        //        WriteCache(cache);
        //    }
        //}

        private static void ReadCache(INeedCache cacheControl, string key)
        {
            var cachePath = GetCachePath(cacheControl, key);

            if(!File.Exists(cachePath))
            {
                return;
            }

            bool success = false;
            try
            {
                using var binReader = new BinaryReader(File.OpenRead(cachePath));
                success = cacheControl.ReadCache(binReader, key);
            }
            catch
            {
                if(!success) throw;
            }
        }

        private static void WriteCache(INeedCache cacheControl, string key)
        {
            var cachePath = GetCachePath(cacheControl, key);

            using MemoryStream mem = new();
            using BinaryWriter binWriter = new(mem);

            if (!cacheControl.WriteCache(binWriter, key)) return;
            using FileStream file = GetCacheFile(cachePath);

            binWriter.Flush();
            mem.WriteTo(file);
        }

        private static FileStream GetCacheFile(string cachePath)
        {
            var cacheDir = Path.GetDirectoryName(cachePath);
            if (!Directory.Exists(cacheDir)) Directory.CreateDirectory(cacheDir);
            return File.OpenWrite(cachePath);
        }

        public static string GetCachePath(INeedCache cachedControl, string key)
        {
            return Path.Combine(CACHE_ROOT, cachedControl.UniquedCachedDir, key) + ".cache";
        }

        private static HashSet<INeedCache> _cacheControls = [];
    }
}
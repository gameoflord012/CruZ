using CruZ.Editor.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CruZ.Editor
{
    /// <summary>
    /// Provide service for read and write caches used in user-space.
    /// </summary>
    class CacheService
    {
        public static string OutputDir = ".";

        public static void Register(ICacheable cacheable)
        {
            _cacheControls.Add(cacheable);

            cacheable.CacheRead -= Cacheable_CacheRead;
            cacheable.CacheRead += Cacheable_CacheRead;

            cacheable.CacheWrite -= Cacheable_CacheWrite;    
            cacheable.CacheWrite += Cacheable_CacheWrite;    
        }

        private static void Cacheable_CacheWrite(ICacheable cache, string key)
        {
            WriteCache(cache, key);
        }

        private static void Cacheable_CacheRead(ICacheable cache, string key)
        {
            ReadCache(cache, key);
        }

        private static void ReadCache(ICacheable cacheControl, string key)
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

        private static void WriteCache(ICacheable cacheControl, string key)
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

        public static string GetCachePath(ICacheable cachedControl, string key)
        {
            return Path.Combine(
                OutputDir, 
                cachedControl.UniquedCachedDir, key) + ".cache";
        }

        static HashSet<ICacheable> _cacheControls = [];
    }
}
using CruZ.Editor.Controls;

using System.Collections.Generic;
using System.IO;

namespace CruZ.Editor.Services
{
    /// <summary>
    /// Provide service for read and write caches
    /// </summary>
    class CacheService
    {
        public string CacheRoot { get; private set; }

        public CacheService(string cacheRoot)
        {
            CacheRoot = cacheRoot;
        }

        public void Register(ICacheable cacheable)
        {
            _cacheControls.Add(cacheable);

            cacheable.CacheRead -= Cacheable_CacheRead;
            cacheable.CacheRead += Cacheable_CacheRead;

            cacheable.CacheWrite -= Cacheable_CacheWrite;    
            cacheable.CacheWrite += Cacheable_CacheWrite;    
        }

        private void Cacheable_CacheWrite(ICacheable cache, string key)
        {
            WriteCache(cache, key);
        }

        private void Cacheable_CacheRead(ICacheable cache, string key)
        {
            ReadCache(cache, key);
        }

        private void ReadCache(ICacheable cacheControl, string key)
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

        private void WriteCache(ICacheable cacheControl, string key)
        {
            var cachePath = GetCachePath(cacheControl, key);

            using MemoryStream mem = new();
            using BinaryWriter binWriter = new(mem);

            if (!cacheControl.WriteCache(binWriter, key)) return;
            using FileStream file = GetCacheFile(cachePath);

            binWriter.Flush();
            mem.WriteTo(file);
        }

        private FileStream GetCacheFile(string cachePath)
        {
            var cacheDir = Path.GetDirectoryName(cachePath);
            if (!Directory.Exists(cacheDir)) Directory.CreateDirectory(cacheDir);
            return File.OpenWrite(cachePath);
        }

        public string GetCachePath(ICacheable cachedControl, string key)
        {
            return Path.Combine(
                CacheRoot, 
                cachedControl.UniquedCachedDir, key) + ".cache";
        }

        HashSet<ICacheable> _cacheControls = [];
    }
}
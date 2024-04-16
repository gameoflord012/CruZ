using System.Collections.Generic;
using System.IO;

namespace CruZ.Editor.Service
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

        public void ReadCache(ICacheable cacheControl, string key)
        {
            var cachePath = GetCachePath(cacheControl, key);

            if (!File.Exists(cachePath))
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
                if (!success) throw;
            }
        }

        public void WriteCache(ICacheable cacheControl, string key)
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
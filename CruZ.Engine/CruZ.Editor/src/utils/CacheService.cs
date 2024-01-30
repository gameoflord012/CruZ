using CruZ.Editor.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace CruZ.Editor
{
    class CacheService
    {
        public const string CACHE_ROOT = "Control Caches";

        public static void RegisterCacheControl(ICacheControl control)
        {
            _cacheControls.Add(control);

            control.CanReadCache -= Control_CanReadCacheChanged;
            control.CanReadCache += Control_CanReadCacheChanged;

            //TODO:
            //control.Control.ParentChanged -= Control_ParentChanged;
            //control.Control.ParentChanged += Control_ParentChanged;
        }

        private static void Control_CanReadCacheChanged(object? sender, bool canRead)
        {
            var cache = (ICacheControl)sender;
            _canReadCaches[cache] = canRead;

            if(canRead)
            {
                ReadCache(cache);
            }
        }

        private static void Control_ParentChanged(object? sender, EventArgs e)
        {
            //TODO:
            //var cache = sender as ICacheControl;
            //Trace.Assert(cache != null);

            //if(cache.Control.Parent != null)
            //{
            //    ReadCache(cache);
            //}
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
            if(!CanRead(cacheControl)) return;

            var cachePath = GetCachePath(cacheControl);
            var cacheDir = Path.GetDirectoryName(cachePath);

            Directory.CreateDirectory(cacheDir);
            if(!File.Exists(cachePath))
            {
                File.Create(cachePath).Close();
            }

            try
            {
                using var file = File.OpenRead(cachePath);
                cacheControl.ReadCache(file);
            }
            catch
            {

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

        private static bool CanRead(ICacheControl cache)
        {
            if(!_canReadCaches.ContainsKey(cache))
            {
                _canReadCaches[cache] = true;
            }

            return _canReadCaches[cache];
        }

        private static HashSet<ICacheControl> _cacheControls = [];
        private static Dictionary<ICacheControl, bool> _canReadCaches = [];
    }
}
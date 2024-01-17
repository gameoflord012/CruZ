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
        //private static List<ICacheControl> CacheControls = [];

        public static void RegisterCacheControl(ICacheControl control)
        {
            _registedCacheControls.Add(control);

            control.CanReadCacheChanged -= Control_CanReadCacheChanged;
            control.CanReadCacheChanged += Control_CanReadCacheChanged;

            control.Control.ParentChanged -= Control_ParentChanged;
            control.Control.ParentChanged += Control_ParentChanged;
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
            var cache = sender as ICacheControl;
            Trace.Assert(cache != null);

            if(cache.Control.Parent != null)
            {
                ReadCache(cache);
            }
        }

        public static void CallReadCaches()
        {
            foreach (var cache in _registedCacheControls)
            {
                ReadCache(cache);
            }
        }

        public static void CallWriteCaches()
        {
            foreach (var cache in _registedCacheControls)
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

            var cacheString = File.ReadAllText(cachePath);
            cacheControl.ReadCache(cacheString);
        }

        private static void WriteCache(ICacheControl cacheControl)
        {
            var cachedString = cacheControl.WriteCache();
            File.WriteAllText(GetCachePath(cacheControl), cachedString);
        }

        private static string GetCachePath(ICacheControl cachedControl)
        {
            return $"Control Caches\\{cachedControl.UniquedCachedPath}";
        }

        private static bool CanRead(ICacheControl cache)
        {
            if(!_canReadCaches.ContainsKey(cache))
            {
                _canReadCaches[cache] = true;
            }

            return _canReadCaches[cache];
        }

        private static HashSet<ICacheControl> _registedCacheControls = [];
        private static Dictionary<ICacheControl, bool> _canReadCaches = [];
    }
}
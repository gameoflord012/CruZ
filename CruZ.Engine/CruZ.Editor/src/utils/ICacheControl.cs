using System;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    public interface ICacheControl
    {
        Control Control { get; }

        event EventHandler<bool> CanReadCacheChanged;

        string UniquedCachedPath { get; }
        string WriteCache();

        void ReadCache(string cacheString);
    }
}
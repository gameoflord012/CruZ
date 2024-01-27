using System;
using System.IO;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    public interface ICacheControl
    {
        Control Control { get; }

        event EventHandler<bool> CanReadCacheChanged;

        string UniquedCachedPath { get; }
        
        void WriteCache(Stream stream);
        void ReadCache(Stream stream);
    }
}
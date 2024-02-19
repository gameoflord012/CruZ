using System;
using System.IO;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    public interface INeedCache
    {
        event Action<INeedCache, string> CacheRead;
        event Action<INeedCache, string> CacheWrite;

        string UniquedCachedDir { get; }
        
        bool WriteCache(BinaryWriter binWriter, string key);
        bool ReadCache(BinaryReader binReader, string key);
    }
}
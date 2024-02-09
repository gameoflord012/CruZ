using System;
using System.IO;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    public interface ICacheControl
    {
        event Action<ICacheControl, string> CacheRead;
        event Action<ICacheControl, string> CacheWrite;

        string UniquedCachedDir { get; }
        
        bool WriteCache(BinaryWriter binWriter, string key);
        bool ReadCache(BinaryReader binReader, string key);
    }
}
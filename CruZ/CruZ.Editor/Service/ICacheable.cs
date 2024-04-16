using System;
using System.IO;
using System.Windows.Forms;

namespace CruZ.Editor.Service
{
    public interface ICacheable
    {
        string UniquedCachedDir { get; }

        bool WriteCache(BinaryWriter binWriter, string key);
        bool ReadCache(BinaryReader binReader, string key);
    }
}
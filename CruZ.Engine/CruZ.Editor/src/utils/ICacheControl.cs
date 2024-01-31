﻿using System;
using System.IO;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    public interface ICacheControl
    {
        event Action<ICacheControl> UpdateCache;
        string UniquedCachedPath { get; }
        
        void WriteCache(Stream stream);
        bool ReadCache(Stream stream);
    }
}
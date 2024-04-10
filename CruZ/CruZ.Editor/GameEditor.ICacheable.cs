﻿using System;
using System.IO;

using CruZ.Editor.Service;
using CruZ.Framework;
using CruZ.Framework.Resource;

namespace CruZ.Editor.Controls
{
    public partial class GameEditor : ICacheable
    {
        public event Action<ICacheable, string>? CacheRead;
        public event Action<ICacheable, string>? CacheWrite;
        public string UniquedCachedDir => "EditorApplication";

        public bool ReadCache(BinaryReader binReader, string key)
        {
            if(key == "LoadedScene")
            {
                var lastSceneFile = binReader.ReadString();

                if (!string.IsNullOrEmpty(lastSceneFile))
                {
                    if(Path.GetExtension(lastSceneFile) == ".scene")
                    {
                        LoadSceneFromFile(lastSceneFile);
                    }
                    else
                    {
                        LoadRuntimeScene(lastSceneFile);
                    }
                }
            }

            if(key == "Camera")
            {
                var px = binReader.ReadSingle();
                var py = binReader.ReadSingle();
                var z = binReader.ReadSingle();

                Camera.Main.CameraOffset = new(px, py);
                Camera.Main.Zoom = z;
            }

            return true;
        }

        public bool WriteCache(BinaryWriter binWriter, string key)
        {
            if(key == "LoadedScene")
            {
                if (CurrentGameScene == null)
                {
                    binWriter.Write("");
                }
                else
                {
                    if(((IResource)CurrentGameScene).Info == null)
                        binWriter.Write(CurrentGameScene.Name); // temporary use name as runtime resource path
                    else
                        binWriter.Write(((IResource)CurrentGameScene).Info.ResourceName);
                }
            }
                
            if(key == "Camera")
            {
                binWriter.Write(Camera.Main.CameraOffset.X);
                binWriter.Write(Camera.Main.CameraOffset.Y);
                binWriter.Write(Camera.Main.Zoom);
            }

            return true;
        }
    }
}
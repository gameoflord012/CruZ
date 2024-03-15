using System;
using System.IO;

using CruZ.Editor.Service;

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
                    if(Path.GetExtension(lastSceneFile) == "scene")
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
                var pz = binReader.ReadSingle();

                var zx = binReader.ReadSingle();
                var zy = binReader.ReadSingle();

                GetMainCamera().Position = new(px, py);
                GetMainCamera().Zoom = new(zx, zy);
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
                    if(CurrentGameScene.ResourceInfo == null)
                        return false;
                    else
                        binWriter.Write(CurrentGameScene.ResourceInfo.ResourceName);
                }
            }
                
            if(key == "Camera")
            {
                binWriter.Write(GetMainCamera().Position.X);
                binWriter.Write(GetMainCamera().Position.Y);

                binWriter.Write(GetMainCamera().Zoom.X);
                binWriter.Write(GetMainCamera().Zoom.Y);
            }

            return true;
        }
    }
}
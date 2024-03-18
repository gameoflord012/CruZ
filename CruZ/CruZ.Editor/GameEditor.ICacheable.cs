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
                var z = binReader.ReadSingle();

                GetMainCamera().CameraOffset = new(px, py);
                GetMainCamera().Zoom = z;
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
                        binWriter.Write(CurrentGameScene.Name); // temporary use name as runtime resource path
                    else
                        binWriter.Write(CurrentGameScene.ResourceInfo.ResourceName);
                }
            }
                
            if(key == "Camera")
            {
                binWriter.Write(GetMainCamera().CameraOffset.X);
                binWriter.Write(GetMainCamera().CameraOffset.Y);
                binWriter.Write(GetMainCamera().Zoom);
            }

            return true;
        }
    }
}
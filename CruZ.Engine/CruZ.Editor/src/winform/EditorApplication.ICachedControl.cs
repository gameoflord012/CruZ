using System;
using System.IO;

namespace CruZ.Editor.Controls
{
    public partial class EditorApplication : ICacheControl
    {
        public event    Action<ICacheControl, string> CacheRead;
        public event    Action<ICacheControl, string> CacheWrite;
        public string   UniquedCachedDir => "EditorApplication";


        public bool ReadCache(BinaryReader binReader, string key)
        {
            if(key == "LoadedScene")
            {
                var lastScenePath = binReader.ReadString();

                if (!string.IsNullOrEmpty(lastScenePath))
                    LoadSceneFromFile(lastScenePath);
            }

            if(key == "Camera")
            {
                var px = binReader.ReadSingle();
                var py = binReader.ReadSingle();
                var pz = binReader.ReadSingle();

                var zx = binReader.ReadSingle();
                var zy = binReader.ReadSingle();
                var zz = binReader.ReadSingle();

                GetMainCamera().Position = new(px, py, pz);
                GetMainCamera().Zoom = new(zx, zy, zz);
            }

            return true;
        }

        public bool WriteCache(BinaryWriter binWriter, string key)
        {
            if(key == "LoadedScene")
            {
                if (CurrentGameScene == null)
                {
                    return false;
                }
                else
                {
                    binWriter.Write(CurrentGameScene.ResourceInfo.ResourceName);
                }
            }
                
            if(key == "Camera")
            {
                binWriter.Write(GetMainCamera().Position.X);
                binWriter.Write(GetMainCamera().Position.Y);
                binWriter.Write(GetMainCamera().Position.Z);

                binWriter.Write(GetMainCamera().Zoom.X);
                binWriter.Write(GetMainCamera().Zoom.Y);
                binWriter.Write(GetMainCamera().Zoom.Z);
            }

            return true;
        }
    }
}
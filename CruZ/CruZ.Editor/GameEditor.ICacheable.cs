using System;
using System.IO;

using CruZ.Editor.Service;
using CruZ.Editor.Winform.Utility;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.Resource;

namespace CruZ.Editor.Controls
{
    public partial class GameEditor : ICacheable
    {
        public string UniquedCachedDir => "EditorApplication";

        bool ICacheable.ReadCache(BinaryReader binReader, string key)
        {
            if(key == "LoadedScene")
            {
                var lastSceneFile = binReader.ReadString();
                try
                {
                    if (!string.IsNullOrEmpty(lastSceneFile))
                    {
                        if (Path.GetExtension(lastSceneFile) == ".scene")
                        {
                            LoadSceneFromFile(lastSceneFile);
                        }
                        else
                        {
                            LoadRuntimeScene(lastSceneFile);
                        }
                    }
                }
                catch (Exception e)
                {
                    DialogHelper.ShowExceptionDialog(e);
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

        bool ICacheable.WriteCache(BinaryWriter binWriter, string key)
        {
            if(key == "LoadedScene")
            {
                if (CurrentGameScene == null)
                {
                    binWriter.Write("");
                }
                else
                {
                    binWriter.Write(CurrentGameScene.Name);
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
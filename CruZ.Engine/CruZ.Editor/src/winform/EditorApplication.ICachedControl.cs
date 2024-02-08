using CruZ.Resource;
using CruZ.Scene;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    public partial class EditorApplication : ICacheControl
    {
        public event    Action<ICacheControl> CacheRead;
        public string   UniquedCachedPath => "WorldViewControlSuperUnique.cache";


        public bool ReadCache(Stream stream)
        {
            using (BinaryReader reader = new(stream))
            {
                // TODO: Looping when calling LoadSceneFromFile

                if(reader.ReadString() != "!WorldViewCache") return false;

                var lastScenePath = reader.ReadString();

                var px = reader.ReadSingle();
                var py = reader.ReadSingle();
                var pz = reader.ReadSingle();

                var zx = reader.ReadSingle();
                var zy = reader.ReadSingle();
                var zz = reader.ReadSingle();


                if (_gameApp == null)
                {
                    if(!string.IsNullOrEmpty(lastScenePath))
                        LoadSceneFromFile(lastScenePath);
                }

                if(_gameApp != null && _gameApp.IsInitialized)
                {
                    GetMainCamera().Position = new(px, py, pz);
                    GetMainCamera().Zoom = new(zx, zy, zz);
                }

                return true;
            }
        }

        public void WriteCache(Stream stream)
        {
            using(BinaryWriter bin = new BinaryWriter(stream))
            {
                bin.Write("!WorldViewCache");

                if(CurrentGameScene == null)
                {
                    bin.Write("");
                }
                else
                {
                    ResourceManager.SaveResource(CurrentGameScene);
                    bin.Write(CurrentGameScene.ResourceInfo.ResourceName);
                }

                bin.Write(GetMainCamera().Position.X);
                bin.Write(GetMainCamera().Position.Y);
                bin.Write(GetMainCamera().Position.Z);

                bin.Write(_mainCamera.Zoom.X);
                bin.Write(_mainCamera.Zoom.Y);
                bin.Write(_mainCamera.Zoom.Z);

                bin.Flush();
            }
        }
    }
}
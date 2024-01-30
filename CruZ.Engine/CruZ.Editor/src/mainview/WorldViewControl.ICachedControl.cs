using CruZ.Resource;
using CruZ.Scene;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    public partial class WorldViewControl : ICacheControl
    {
        //public Control Control => this;
        public event    EventHandler<bool> CanReadCache;
        public string   UniquedCachedPath => "WorldViewControlSuperUnique.cache";


        public void ReadCache(Stream stream)
        {
            using (BinaryReader reader = new(stream))
            {
                if(reader.ReadString() != "!WorldViewCache") return;

                var lastScenePath = reader.ReadString();

                _mainCamera.Position = new(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                    );

                _mainCamera.Zoom = new(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                    );
                
                try
                {
                    var toLoad = ResourceManager.LoadResource<GameScene>(lastScenePath);
                    LoadScene(toLoad);
                }
                catch 
                {
                    throw;
                }
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

                bin.Write(_mainCamera.Position.X);
                bin.Write(_mainCamera.Position.Y);
                bin.Write(_mainCamera.Position.Z);

                bin.Write(_mainCamera.Zoom.X);
                bin.Write(_mainCamera.Zoom.Y);
                bin.Write(_mainCamera.Zoom.Z);

                bin.Flush();
            }
        }
    }
}
using CruZ.Resource;
using CruZ.Scene;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    internal partial class WorldViewControl : ICacheControl
    {
        public Control Control => this;
        public event    EventHandler<bool> CanReadCacheChanged;
        public string   UniquedCachedPath => "WorldViewControlSuperUnique.cache";


        public void ReadCache(Stream stream)
        {
            using (BinaryReader reader = new(stream))
            {
                if(reader.ReadString() != "!WorldViewCache") return;

                var lastScenePath = reader.ReadString();

                _camera.Position = new(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                    );

                _camera.Zoom = new(
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

                bin.Write(_camera.Position.X);
                bin.Write(_camera.Position.Y);
                bin.Write(_camera.Position.Z);

                bin.Write(_camera.Zoom.X);
                bin.Write(_camera.Zoom.Y);
                bin.Write(_camera.Zoom.Z);

                bin.Flush();
            }
        }
    }
}
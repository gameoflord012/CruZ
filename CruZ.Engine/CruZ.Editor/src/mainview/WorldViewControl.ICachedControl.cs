using CruZ.Resource;
using CruZ.Scene;
using System;
using System.IO;
using System.Windows.Forms;

namespace CruZ.Editor.Controls
{
    internal partial class WorldViewControl : ICacheControl
    {
        public Control Control => this;
        public event EventHandler<bool> CanReadCacheChanged;
        public string UniquedCachedPath => "WorldViewControlSuperUnique.cache";


        public void ReadCache(string cacheString)
        {
            using (StringReader reader = new(cacheString))
            {
                var lastScenePath = reader.ReadLine();
                
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

        public string WriteCache()
        {
            if(CurrentGameScene == null) return "";
            ResourceManager.SaveResource(CurrentGameScene);
            return CurrentGameScene.ResourceInfo.ResourceName;
        }
    }
}
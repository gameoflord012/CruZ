using CruZ.Resource;
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
                    LoadScene(ResourceManager.LoadResource<GameScene>(lastScenePath));
                }
                catch 
                {
                    throw;
                }
            }
        }

        public string WriteCache()
        {
            return CurrentGameScene != null ? CurrentGameScene.ResourcePath : "";
        }

    }
}
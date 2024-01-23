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
                    GameScene toLoad;

                    if(Path.GetExtension(lastScenePath) != "scene")
                    {
                        toLoad = SceneManager.GetSceneAssets(lastScenePath);
                    }
                    else
                    {
                        toLoad = ResourceManager.LoadResource<GameScene>(lastScenePath);
                    }

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
            return CurrentGameScene != null ? CurrentGameScene.ResourcePath : "";
        }

    }
}
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

                    if(!IsSceneFromFile(lastScenePath))
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
            if(CurrentGameScene == null) return "";
            return CurrentGameScene.ResourcePath;


            if(IsSceneFromFile(CurrentGameScene))
                return CurrentGameScene.ResourcePath;
            else
            {
                CurrentGameScene.ResourcePath = Path.Combine(
                    CacheService.CACHE_ROOT, "scenes", CurrentGameScene.ResourcePath + ".scene");

                ResourceManager.CreateResource(CurrentGameScene.ResourcePath, false);

                return CurrentGameScene.ResourcePath;
            }
        }

        private bool IsSceneFromFile(GameScene scene)
        {
            return IsSceneFromFile(scene.ResourcePath);
        }

        private bool IsSceneFromFile(string scenePath)
        {
            return Path.GetExtension(scenePath) == "scene";
        }
    }
}
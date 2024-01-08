//using CruZ.Utility;
//using Microsoft.Xna.Framework;
//using MonoGame.Forms.NET.Controls;


//// TODO - change imgui to winform application
//namespace CruZ.Editor
//{
//    class Editor : MonoGameControl
//    {
//        protected override void Initialize()
//        {
//            CreateViews();
//            LoadViewCaches(_viewsToAdd);
//        }

//        private void CreateViews()
//        {
//            SceneManager.OnSceneLoaded += CreateEntityViews;
//            SceneManager.OnCurrentSceneUnLoaded += RemoveEntityViews;

//            AddView(new SceneSelectionView());
//            AddView(new LoggingView());
//            AddView(new CameraView());
//        }

//        private void CreateEntityViews(GameScene scene)
//        {
//            foreach(var e in scene.Entities)
//            {
//                var view = new EntityView();
//                view.Binding = e;
//                AddView(view);
//            }
//        }
//        private void RemoveEntityViews(GameScene scene)
//        {
//            _viewsToRemove.AddRange(_views.Where(v => v is EntityView));
//        }

//        private void LoadViewCaches(List<object> viewsToLoad)
//        {
//            for (int i = 0; i < viewsToLoad.Count; i++)
//            {
//                var loadedView = Helper.Serializer.DeserializeFromFile(
//                    GetSerializePath(viewsToLoad[i]), viewsToLoad[i].GetType());

//                if (loadedView == null) continue;
//                viewsToLoad[i] = loadedView;
//            }
//        }

//        protected override void LateDraw(GameTime gameTime)
//        {
//            EntityView.InitializeWindow();

//            foreach (var view in _views)
//            {
//                if (view is IViewDrawCallback)
//                    ((IViewDrawCallback)view).DrawView(gameTime);
//            }

//            UpdateViewList();
//        }

//        private void UpdateViewList()
//        {
//            _views = _views.Except(_viewsToAdd).Except(_viewsToRemove).ToList();
//            _views.AddRange(_viewsToAdd);

//            _viewsToAdd.Clear();
//            _viewsToRemove.Clear();
//        }

//        private void AddView(IViewDrawCallback view)
//        {
//            _viewsToAdd.Add(view);
//        }

//        protected override void Exit(object sender, EventArgs args)
//        {
//            for (int i = 0; i < _views.Count; i++)
//            {
//                Helper.Serializer.SerializeToFile(_views[i], GetSerializePath(_views[i]));
//            }
//        }

//        private string GetSerializePath(object o)
//        {
//            return string.Format("Editor\\{0}.json", o.GetType().ToString());
//        }

//        protected override void Update(GameTime gameTime)
//        {
//            throw new NotImplementedException();
//        }

//        protected override void Draw()
//        {
//            throw new NotImplementedException();
//        }

//        private List<object> _views = new();
//        private List<object> _viewsToAdd = new();
//        private List<object> _viewsToRemove = new();
//        private static uint _dockSpaceId;
//    }
//}
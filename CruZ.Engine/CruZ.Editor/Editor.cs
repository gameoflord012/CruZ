using CruZ.UI;
using CurZ;
using CurZ.Editor;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace CruZ.Editor
{
    class Editor : CruZGame
    {
        public override void Initialize()
        {
            base.Initialize();

            _imgui = new(Core);
            _imgui.RebuildFontAtlas();

            CreateViews();
            LoadViewCaches(_viewsToAdd);
        }

        private void CreateViews()
        {
            SceneManager.OnSceneLoaded += CreateEntityViews;
            SceneManager.OnCurrentSceneUnLoaded += RemoveEntityViews;

            AddView(new SceneSelectionView());
            AddView(new LoggingView());
            AddView(new CameraView());
        }

        private void CreateEntityViews(GameScene scene)
        {
            foreach(var e in scene.Entities)
            {
                var view = new EntityView();
                view.Binding = e;
                AddView(view);
            }
        }
        private void RemoveEntityViews(GameScene scene)
        {
            _viewsToRemove.AddRange(_views.Where(v => v is EntityView));
        }

        private void LoadViewCaches(List<object> viewsToLoad)
        {
            for (int i = 0; i < viewsToLoad.Count; i++)
            {
                var loadedView = GlobalSerializer.DeserializeFromFile(
                    GetSerializePath(viewsToLoad[i]), viewsToLoad[i].GetType());

                if (loadedView == null) continue;
                viewsToLoad[i] = loadedView;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            _imgui.BeforeLayout(gameTime);

            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport(), ImGuiDockNodeFlags.PassthruCentralNode);
            EntityView.InitializeWindow();

            foreach (var view in _views)
            {
                if (view is IViewDrawCallback)
                    ((IViewDrawCallback)view).DrawView(gameTime);
            }
            _imgui.AfterLayout();

            UpdateViewList();
        }

        private void UpdateViewList()
        {
            _views = _views.Except(_viewsToAdd).Except(_viewsToRemove).ToList();
            _views.AddRange(_viewsToAdd);

            _viewsToAdd.Clear();
            _viewsToRemove.Clear();
        }

        private void AddView(IViewDrawCallback view)
        {
            _viewsToAdd.Add(view);
        }

        protected override void OnExit(object sender, EventArgs args)
        {
            for (int i = 0; i < _views.Count; i++)
            {
                GlobalSerializer.SerializeToFile(_views[i], GetSerializePath(_views[i]));
            }
        }

        private string GetSerializePath(object o)
        {
            return string.Format("Editor\\{0}.json", o.GetType().ToString());
        }

        private ImGuiRenderer _imgui;
        private List<object> _views = new();
        private List<object> _viewsToAdd = new();
        private List<object> _viewsToRemove = new();
        private static uint _dockSpaceId;

        public static uint DockSpaceId { get => _dockSpaceId; }
    }
}
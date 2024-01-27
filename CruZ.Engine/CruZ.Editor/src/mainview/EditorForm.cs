using CruZ.Editor.Controls;
using CruZ.Resource;
using CruZ.Scene;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public partial class EditorForm : Form
    {
        public PropertyGrid Inspector_PropertyGrid  { get => inspector_PropertyGrid; }
        public WorldViewControl WorldViewControl    { get => worldViewControl; }

        private EditorForm()
        {
            InitializeComponent();
            worldViewControl.OnSelectedEntityChanged += WorldViewControl_OnSelectedEntityChanged;
        }

        private void WorldViewControl_OnSelectedEntityChanged(object? sender, Components.TransformEntity e)
        {
            Inspector.Instance.DisplayEntity(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CacheService.CallWriteCaches();
        }

        private void OpenScene_Clicked(object sender, EventArgs e)
        {
            var files = DialogHelper.SelectSceneFile(false);
            if (files.Count() == 0) return;

            string sceneFile = files[0];

            var scene = ResourceManager.LoadResource<GameScene>(sceneFile, out _);
            worldViewControl.LoadScene(scene);
        }

        private void SaveScene_Clicked(object sender, EventArgs args)
        {
            if (worldViewControl.CurrentGameScene == null) return;

            try
            {
                ResourceManager.SaveResource(worldViewControl.CurrentGameScene);
            }
            catch(System.Exception e)
            {
                ShowExceptionDialog(e);
            }

        }

        private void File_Menu_Clicked(object sender, EventArgs e)
        {
        }

        private void SaveAsScene_Clicked(object sender, EventArgs e)
        {
            var savePath = DialogHelper.GetSaveScenePath();
            if (savePath == null) return;

            ResourceManager.CreateResource(
                savePath,
                worldViewControl.CurrentGameScene, 
                true);
        }

        private void LoadScene_Clicked(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter Scene name to load", "Load scene Prompt");

            if(string.IsNullOrWhiteSpace(input)) return;

            try
            {
                worldViewControl.LoadScene(SceneManager.GetSceneAssets(input));
            }
            catch (SceneAssetNotFoundException ex)
            {
                ShowExceptionDialog(ex);
            }
        }

        private void ShowExceptionDialog(System.Exception ex)
        {
            MessageBox.Show(
                $"{ex}\nInner Error: {ex.InnerException}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
        
        static EditorForm? _instance;
        public static EditorForm Instance => _instance ??= new EditorForm();
    }
}

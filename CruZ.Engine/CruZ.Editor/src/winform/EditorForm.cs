using CruZ.Editor.Controls;
using CruZ.Editor.Systems;
using CruZ.Exception;
using CruZ.Resource;
using CruZ.Scene;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public partial class EditorForm : Form
    {
        public static event Action FormClosing;

        public PropertyGrid Inspector_PropertyGrid { get => inspector_PropertyGrid; }
        //TODO: public EditorApplication EditorApplication    { get => _editorApp; }

        private EditorForm()
        {
            KeyPreview = true;

            InitializeComponent();
            _editorApp = new();

            _editorApp.SelectEntityChanged += EditorApp_SelectEntity;
            _editorApp.LoadedSceneChanged += EditorApp_LoadNewScene;

            entities_ComboBox.SelectedIndexChanged += Entities_ComboBox_SelectedIndexChanged;
            entities_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void Init()
        {
            _editorApp.Init();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Z))
            {
                UndoService.Undo();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.Z))
            {
                UndoService.Redo();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void EditorApp_LoadNewScene(GameScene? scene)
        {
            entities_ComboBox.Items.Clear();

            if(scene == null) return;

            for (int i = 0; i < scene.Entities.Count(); i++)
            {
                entities_ComboBox.Items.Add(scene.Entities[i]);
            }
        }

        private void EditorApp_SelectEntity(Components.TransformEntity? e)
        {
            Inspector.DisplayEntity(e);
            //Trace.Assert(entities_ComboBox.Items.Contains(e));
            //entities_ComboBox.SelectedItem = e;
        }

        #region Form_Events_Handler
        private void Entities_ComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            //TODO: _editorApp.SelectEntity((TransformEntity)entities_ComboBox.SelectedItem);
        }

        private void OpenScene_Clicked(object sender, EventArgs e)
        {
            var files = DialogHelper.SelectSceneFile(false);
            if (files.Count() == 0) return;

            string sceneFile = files[0];

            _editorApp.LoadSceneFromFile(sceneFile);
        }

        private void SaveScene_Clicked(object sender, EventArgs args)
        {
            //TODO: if (_editorApp.CurrentGameScene == null) return;

            try
            {
                //TODO: ResourceManager.SaveResource(_editorApp.CurrentGameScene);
            }
            catch (System.Exception e)
            {
                DialogHelper.ShowExceptionDialog(e);
            }

        }

        private void File_Menu_Clicked(object sender, EventArgs e)
        {

        }

        private void SaveAsScene_Clicked(object sender, EventArgs e)
        {
            if (_editorApp.CurrentGameScene == null)
            {
                DialogHelper.ShowInfoDialog("Nothing to save.");
                return;
            }

            var savePath = DialogHelper.GetSaveScenePath();
            if (savePath == null) return;

            ResourceManager.CreateResource(
                savePath,
                _editorApp.CurrentGameScene,
                true);
        }

        private void LoadScene_Clicked(object sender, EventArgs e)
        {
            //string sceneName = Microsoft.VisualBasic.Interaction.InputBox(
            //    "Enter Scene name to load", "Load scene Prompt");

            using var dialog = new LoadRuntimeSceneForm();
            dialog.ShowDialog();

            if (dialog.DialogResult != DialogResult.OK) return;

            var sceneName = dialog.ReturnSceneName;

            if (string.IsNullOrWhiteSpace(sceneName)) return;

            try
            {
                _editorApp.LoadRuntimeScene(sceneName);
            }
            catch (SceneAssetNotFoundException ex)
            {
                DialogHelper.ShowExceptionDialog(ex);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //CacheService.CallWriteCaches();
            FormClosing?.Invoke();
            _editorApp.ExitApp();
        } 
        #endregion

        EditorApplication _editorApp;
        
        #region Static
        public static PropertyGrid GetPropertyGrid()
        {
            return _instance.inspector_PropertyGrid;
        }

        public static void Run()
        {
            if (_instance != null) throw new InvalidOperationException("Already Ran");

            _instance = new EditorForm();
            _instance.Init();

            Application.Run(_instance);
        }

        static EditorForm? _instance; 
        #endregion
    }
}

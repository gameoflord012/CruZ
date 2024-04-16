using CruZ.Editor.Controls;
using CruZ.Editor.Service;
using CruZ.Editor.Winform.Utility;
using CruZ.Framework;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CruZ.Editor
{
    /// <summary>
    /// Handle WinForm API for the editor
    /// </summary>
    public partial class EditorForm : Form
    {
        private EditorForm()
        {
            InitializeComponent();

            Text = "CruZ Engine";
            _gameEditor = new(this);
            entityInspector.Editor = _gameEditor;
            sceneEditor.Editor = _gameEditor;
        }

        #region Overrides
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

        protected override void OnLoad(EventArgs e)
        {
            _gameEditor.LoadLastSessionCaches();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _gameEditor.CleanAppSession();
            //_gameEditor.CurrentSceneChanged -= EditorApp_CurrentSceneChanged;
        }
        #endregion

        #region Clicked Event Handlers
        private void OpenScene_Clicked(object sender, EventArgs e)
        {
            var files = DialogHelper.SelectSceneFile(false);
            if (files.Count() == 0) return;

            string sceneFile = files[0];

            _gameEditor.LoadSceneFromFile(sceneFile);
        }

        private void SaveScene_Clicked(object sender, EventArgs args)
        {
            //TODO: if (_gameEditor.CurrentGameScene == null) return;

            try
            {
                //TODO: ResourceManager.Save(_gameEditor.CurrentGameScene);
            }
            catch (Exception e)
            {
                DialogHelper.ShowExceptionDialog(e);
            }
        }

        private void SaveAsScene_Clicked(object sender, EventArgs e)
        {
            if (_gameEditor.CurrentGameScene == null)
            {
                DialogHelper.ShowInfoDialog("Nothing to save.");
                return;
            }

            var savePath = DialogHelper.GetSaveScenePath();
            if (string.IsNullOrEmpty(savePath)) return;

            EditorContext.UserResource.Create(
                savePath,
                _gameEditor.CurrentGameScene);
        }

        private void LoadScene_Clicked(object sender, EventArgs e)
        {
            using var dialog = new LoadRuntimeSceneDialog();
            dialog.ShowDialog();

            if (dialog.DialogResult != DialogResult.OK) return;

            var sceneName = dialog.ReturnSceneName;

            if (string.IsNullOrWhiteSpace(sceneName)) return;

            try
            {
                _gameEditor.LoadRuntimeScene(sceneName);
            }
            catch (SceneAssetNotFoundException ex)
            {
                DialogHelper.ShowExceptionDialog(ex);
            }
        }

        #endregion

        #region Private
        GameEditor _gameEditor;
        #endregion

        #region Static
        public static void Run()
        {
            if (_instance != null) throw new InvalidOperationException("Already Ran");
            _instance = new EditorForm();
            Application.Run(_instance);
        }

        static EditorForm? _instance;
        #endregion
    }
}

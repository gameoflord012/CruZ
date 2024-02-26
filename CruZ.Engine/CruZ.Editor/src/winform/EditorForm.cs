using CruZ.Components;
using CruZ.Editor.Controls;
using CruZ.Editor.Services;
using CruZ.Editor.Utility;
using CruZ.Exception;
using CruZ.Resource;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CruZ.Editor
{
    public partial class EditorForm : Form
    {
        private EditorForm()
        {
            InitializeComponent();

            Text = "CruZ Engine";

            _editor = new(this);
            _formThread = Thread.CurrentThread;
        }

        private void Init()
        {
            _editor.Init();
            entityInspector.Init(_editor);
            sceneEditor.Init(_editor);
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _editor.CleanAppSession();
            //_editor.CurrentSceneChanged -= EditorApp_CurrentSceneChanged;
        }
        #endregion

        #region Clicked Event Handlers
        private void OpenScene_Clicked(object sender, EventArgs e)
        {
            var files = DialogHelper.SelectSceneFile(false);
            if (files.Count() == 0) return;

            string sceneFile = files[0];

            _editor.LoadSceneFromFile(sceneFile);
        }

        private void SaveScene_Clicked(object sender, EventArgs args)
        {
            //TODO: if (_editor.CurrentGameScene == null) return;

            try
            {
                //TODO: ResourceManager.SaveResource(_editor.CurrentGameScene);
            }
            catch (System.Exception e)
            {
                DialogHelper.ShowExceptionDialog(e);
            }
        }

        private void SaveAsScene_Clicked(object sender, EventArgs e)
        {
            if (_editor.CurrentGameScene == null)
            {
                DialogHelper.ShowInfoDialog("Nothing to save.");
                return;
            }

            var savePath = DialogHelper.GetSaveScenePath();
            if (savePath == null) return;

            ResourceManager.User.CreateResource(
                savePath,
                _editor.CurrentGameScene,
                true);

            _editor.LoadSceneFromFile(savePath);
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
                _editor.LoadRuntimeScene(sceneName);
            }
            catch (SceneAssetNotFoundException ex)
            {
                DialogHelper.ShowExceptionDialog(ex);
            }
        }

        #endregion

        #region Private
        Thread _formThread;
        GameEditor _editor;
        #endregion

        #region Static
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

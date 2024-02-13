using Assimp;
using CruZ.Components;
using CruZ.Editor.Controls;
using CruZ.Editor.Systems;
using CruZ.Editor.Utility;
using CruZ.Exception;
using CruZ.Resource;
using CruZ.Scene;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            KeyPreview = true;

            InitializeComponent();

            _editorApp = new(this);

            _editorApp.SelectEntityChanged += EditorApp_SelectEntity;
            _editorApp.LoadedSceneChanged += EditorApp_LoadNewScene;

            entities_ComboBox.SelectedIndexChanged += EntityComboBox_SelectedIndexChanged;
            sceneTree.BeforeSelect += SceneTree_BeforeSelect;

            _formThread = Thread.CurrentThread;
            
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

        #region Components_Event_Handlers
        private void SceneTree_BeforeSelect(object? sender, TreeViewCancelEventArgs e)
        {
            _editorApp.SelectEntity((TransformEntity)e.Node.Tag);
        }

        private void EditorApp_LoadNewScene(GameScene? scene)
        {
            UpdateEntityComboBox(scene);
            UpdateSceneTree(scene);
        }

        private void EditorApp_SelectEntity(Components.TransformEntity? e)
        {
            SafeInvoke(entities_ComboBox, 
                () => entities_ComboBox.SelectedItem = e);

            UpdatePropertyGrid(e);
        }

        private void EntityComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _editorApp.SelectEntity((TransformEntity)entities_ComboBox.SelectedItem);
        } 
        #endregion

        #region Clicked_Event_Handlers

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

        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _editorApp.CleanAppSession();

            _editorApp.SelectEntityChanged -= EditorApp_SelectEntity;
            _editorApp.LoadedSceneChanged -= EditorApp_LoadNewScene;
            GameApplication.UnregisterDraw(GameApp_Draw);
        }

        #region Private
        private void UpdateEntityComboBox(GameScene? scene)
        {
            SafeInvoke(entities_ComboBox, delegate
            {
                entities_ComboBox.Items.Clear();

                if (scene == null) return;

                for (int i = 0; i < scene.Entities.Count(); i++)
                {
                    var e = scene.Entities[i];
                    entities_ComboBox.Items.Add(e);
                }
            });
        }

        private void UpdateSceneTree(GameScene? scene)
        {
            SafeInvoke(sceneTree, delegate
            {
                sceneTree.Nodes.Clear();

                if (scene == null) return;

                for (int i = 0; i < scene.Entities.Count(); i++)
                {
                    var e = scene.Entities[i];

                    sceneTree.Nodes.Add(e.ToString());
                    sceneTree.Nodes[i].ContextMenuStrip = sceneContextMenuStrip;
                    sceneTree.Nodes[i].Tag = e;
                }
            });
        }

        public void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
        #endregion

        EditorApplication _editorApp;
        Thread _formThread;

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

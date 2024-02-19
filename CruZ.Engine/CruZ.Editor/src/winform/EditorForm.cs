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
        //internal event Action<TransformEntity?>? SelectingEntityChanged;
        //internal event Action<GameScene?>? CurrentSceneChanged;

        private EditorForm()
        {
            InitializeComponent();

            KeyPreview = true;
            Text = "CruZ Engine";

            _editor = new(this);
            _editor.CurrentSceneChanged += EditorApp_LoadNewScene;

            _formThread = Thread.CurrentThread;

            _editor.SelectingEntityChanged += Editor_SelectingEntityChanged;

            InitSceneTree();

            componentEditor_ToolStripMenuItem.Click += AddComponent_Click;

            addEntity_ToolStripMenuItem.Click += AddEntity_Click;
        }

        private void InitSceneTree()
        {
            sceneTree.HideSelection = false;
            sceneTree.BeforeSelect += SceneTree_BeforeSelect;
            sceneTree.ContextMenuStrip = scene_ContextMenuStrip;
            sceneTree.NodeMouseClick += (sender, args)
                => sceneTree.SelectedNode = args.Node;
        }

        private void Editor_SelectingEntityChanged(TransformEntity? e)
        {
            sceneTree.SafeInvoke(delegate
            {
                sceneTree.SelectedNode = e == null ? null : _entityToNode[e];
            });
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
            _editor.CurrentSceneChanged -= EditorApp_LoadNewScene;
        }
        #endregion
        #region Components_Event_Handlers
        private void SceneTree_BeforeSelect(object? sender, TreeViewCancelEventArgs e)
        {
            _editor.SelectEntity((TransformEntity)e.Node.Tag);
        }

        private void EditorApp_LoadNewScene(GameScene? scene)
        {
            UpdateSceneTree(scene);
        }
        #endregion

        #region Clicked_Event_Handlers
        private void AddEntity_Click(object? sender, EventArgs e)
        {
            
        }

        private void AddComponent_Click(object? sender, EventArgs args)
        {
            var menuItem = (ToolStripMenuItem)sender;
            var context = (ContextMenuStrip)menuItem.Owner;

            
            TreeView tree = (TreeView)context.SourceControl;
            var e = (TransformEntity)tree.SelectedNode.Tag;

            var editCompDialog = new EditComponentDialog(e);
            editCompDialog.ShowDialog();

            Debug.WriteLine(e);
        }

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

        private void File_Menu_Clicked(object sender, EventArgs e)
        {

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

            ResourceManager.CreateResource(
                savePath,
                _editor.CurrentGameScene,
                true);

            _editor.LoadSceneFromFile(savePath);
        }

        private void LoadScene_Clicked(object sender, EventArgs e)
        {
            //string sceneName = Microsoft.VisualBasic.Interaction.InputBox(
            //    "Enter Scene name to load", "Load scene Prompt");

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
        private void UpdateSceneTree(GameScene? scene)
        {
            sceneTree.SafeInvoke(delegate
            {
                sceneTree.Nodes.Clear();
                _entityToNode.Clear();

                if (scene == null) return;
                
                sceneTree.Nodes.Add(scene.ToString());
                var root = sceneTree.Nodes[0];
                root.ContextMenuStrip = scene_ContextMenuStrip;

                for (int i = 0; i < scene.Entities.Count(); i++)
                {
                    var e = scene.Entities[i];
                    root.Nodes.Add(e.ToString());

                    var node = root.Nodes[i];

                    node.ContextMenuStrip = entity_ContextMenuStrip;
                    node.Tag = e;

                    _entityToNode[e] = node;
                }
            });
        }

        private void Init()
        {
            _editor.Init();
            entityInspector.Init(_editor);
        }

        EditorApplication _editor;
        Thread _formThread;
        Dictionary<TransformEntity, TreeNode> _entityToNode = [];
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

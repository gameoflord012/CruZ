using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using CruZ.Components;
using CruZ.Editor.Controls;
using CruZ.Editor.Services;
using CruZ.Editor.Utility;

namespace CruZ.Editor
{
    public partial class SceneEditor : UserControl
    {
        public SceneEditor()
        {
            InitializeComponent();

            scene_TreeView.LabelEdit = true;
            scene_TreeView.HideSelection = false;
            scene_TreeView.BeforeSelect += SceneTree_BeforeSelect;
            scene_TreeView.NodeMouseClick += (sender, args)
                => scene_TreeView.SelectedNode = args.Node;
            scene_TreeView.BeforeLabelEdit += SceneTree_BeforeLabelEdit;
            scene_TreeView.AfterLabelEdit += SceneTree_AfterLabelEdit;

            editEntity_ToolStripMenuItem.Click += EditEntity_ToolStripMenuItem_Clicked;
        }

        public void Init(GameEditor editor)
        {
            FindForm().FormClosing += EditorForm_FormClosing;

            _editor = editor;
            _editor.SelectingEntityChanged += EditorApp_SelectedEntityChanged;
            _editor.CurrentSceneChanged += EditorApp_CurrentSceneChanged;

            UpdateSceneTree(_editor.SelectedEntity);
            UpdateSceneTree(_editor.CurrentGameScene);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.F2)
            {
                if(scene_TreeView.SelectedNode != null)
                {
                    scene_TreeView.SelectedNode.BeginEdit();
                }
            }

            return false;
        }

        #region Event Handlers
        private void SceneTree_BeforeSelect(object? sender, TreeViewCancelEventArgs e)
        {
            _editor.SelectedEntity = (TransformEntity)e.Node.Tag;
        }

        private void SceneTree_BeforeLabelEdit(object? sender, NodeLabelEditEventArgs args)
        {
            if (args.Node == _root_TreeNode) args.CancelEdit = true;
        }

        private void SceneTree_AfterLabelEdit(object? sender, NodeLabelEditEventArgs args)
        {
            if(string.IsNullOrEmpty(args.Label))
            {
                return;
            }

            var e = (TransformEntity)args.Node.Tag;
            e.Name = args.Label;
            InvalidateService.Invalidate(InvalidatedEvents.EntityNameChanged);
        }

        private void EditorApp_SelectedEntityChanged(TransformEntity? e)
        {
            UpdateSceneTree(e);
        }

        private void EditorApp_CurrentSceneChanged(GameScene? scene)
        {
            UpdateSceneTree(scene);
        }

        private void EditorForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _editor.CurrentSceneChanged -= EditorApp_CurrentSceneChanged;
        }

        private void EditEntity_ToolStripMenuItem_Clicked(object? sender, EventArgs args)
        {
            var menuItem = (ToolStripMenuItem)sender;
            var context = (ContextMenuStrip)menuItem.Owner;


            TreeView tree = (TreeView)context.SourceControl;
            var e = (TransformEntity)tree.SelectedNode.Tag;

            var editCompDialog = new EditComponentDialog(e);
            editCompDialog.ShowDialog();
        } 
        #endregion

        #region Private Functions
        private void UpdateSceneTree(TransformEntity? selectedEntity)
        {
            scene_TreeView.SafeInvoke(delegate
            {
                scene_TreeView.SelectedNode = selectedEntity == null ? null : _entityToNode[selectedEntity];
            });
        }

        private void UpdateSceneTree(GameScene? currentScene)
        {
            scene_TreeView.SafeInvoke(delegate
            {
                scene_TreeView.Nodes.Clear();
                _entityToNode.Clear();
                _root_TreeNode = null;

                if (currentScene == null) return;

                scene_TreeView.Nodes.Add(currentScene.ToString());
                _root_TreeNode = scene_TreeView.Nodes[0];
                _root_TreeNode.ContextMenuStrip = null;

                for (int i = 0; i < currentScene.Entities.Count(); i++)
                {
                    var e = currentScene.Entities[i];
                    _root_TreeNode.Nodes.Add(e.ToString());

                    var entityNode = _root_TreeNode.Nodes[i];

                    entityNode.ContextMenuStrip = sceneEntity_ContextMenuStrip;
                    entityNode.Tag = e;

                    _entityToNode[e] = entityNode;
                }
            });
        } 
        #endregion

        TreeNode? _root_TreeNode;
        GameEditor _editor;
        Dictionary<TransformEntity, TreeNode> _entityToNode = [];
    }
}

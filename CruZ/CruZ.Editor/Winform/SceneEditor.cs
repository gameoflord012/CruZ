using System;
using System.Collections.Generic;
using System.Windows.Forms;

using CruZ.Common.Scene;
using CruZ.Editor.Controls;
using CruZ.Editor.Service;
using CruZ.Editor.Winform.Utility;
using CruZ.Framework.GameSystem.ECS;

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
            scene_TreeView.NodeMouseClick += (sender, args) => scene_TreeView.SelectedNode = args.Node;
            scene_TreeView.BeforeLabelEdit += SceneTree_BeforeLabelEdit;
            scene_TreeView.AfterLabelEdit += SceneTree_AfterLabelEdit;

            addEntity_ToolStripMenuItem.Click += AddEntity_ToolStripMenuItem_Click;
            editEntity_ToolStripMenuItem.Click += EditEntity_ToolStripMenuItem_Clicked;
            removeEntity_ToolStripMenuItem.Click += RemoveEntity_ToolStripMenuItem_Click;
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
            if (string.IsNullOrEmpty(args.Label))
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
            var e = (TransformEntity)scene_TreeView.SelectedNode.Tag;
            var editCompDialog = new EditComponentDialog(e);
            editCompDialog.ShowDialog();
        }
        
        private void AddEntity_ToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            _editor.CreateNewEntity();
        }

        private void RemoveEntity_ToolStripMenuItem_Click(object? sender, EventArgs args)
        {
            var e = (TransformEntity)scene_TreeView.SelectedNode.Tag;
            _editor.RemoveEntity(e);
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
                if(scene_TreeView.Tag == currentScene) return;

                if(scene_TreeView.Tag != null)
                {
                    var oldScene = (GameScene)scene_TreeView.Tag;
                    oldScene.EntityAdded -= CurrentScene_EntityAdded;
                    oldScene.EntityRemoved -= CurrentScene_EntityRemoved;
                }

                scene_TreeView.Tag = currentScene;
                scene_TreeView.Nodes.Clear();
                _entityToNode.Clear();
                _root_TreeNode = null;

                if (currentScene == null) return;

                currentScene.EntityAdded += CurrentScene_EntityAdded;
                currentScene.EntityRemoved += CurrentScene_EntityRemoved;

                // init tree root
                _root_TreeNode = scene_TreeView.Nodes.Add(currentScene.ToString());
                _root_TreeNode.ContextMenuStrip = sceneRoot_ContextMenuStrip;

                // update new added entity
                foreach (var e in currentScene.Entities)
                {
                    AddSceneNode(e);
                }
            });
        }

        void CurrentScene_EntityAdded(TransformEntity entity)
        {
            AddSceneNode(entity);
        }

        void CurrentScene_EntityRemoved(TransformEntity entity)
        {
            RemoveSceneNode(entity);
        }

        private void AddSceneNode(TransformEntity e)
        {
            if(_entityToNode.ContainsKey(e)) return;

            var entityNode = _root_TreeNode.Nodes.Add(e.ToString());
            entityNode.ContextMenuStrip = sceneEntity_ContextMenuStrip;
            entityNode.Tag = e;

            _entityToNode[e] = entityNode;
        }

        private void RemoveSceneNode(TransformEntity e)
        {
            _entityToNode[e].Remove();
            _entityToNode.Remove(e);
        }
        #endregion

        TreeNode? _root_TreeNode;
        GameEditor _editor;
        Dictionary<TransformEntity, TreeNode> _entityToNode = [];
    }
}

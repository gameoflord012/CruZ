using System;
using System.Collections.Generic;
using System.Windows.Forms;

using CruZ.Editor.Controls;
using CruZ.Editor.Service;
using CruZ.Editor.Winform.Utility;
using CruZ.GameEngine.GameSystem;
using CruZ.GameEngine.GameSystem.Scene;

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

            addEntity_SceneRoot_ToolStripMenuItem.Click += AddEntity_ToolStripMenuItem_Click;
            addEntity_RootChild_ToolStripMenuItem.Click += AddEntity_ToolStripMenuItem_Click;
            editEntity_ToolStripMenuItem.Click += EditEntity_ToolStripMenuItem_Clicked;
            removeEntity_ToolStripMenuItem.Click += RemoveEntity_ToolStripMenuItem_Click;
        }

        private void SceneTree_BeforeSelect(object? sender, TreeViewCancelEventArgs e)
        {
            Editor.SelectedEntity = (TransformEntity)e.Node.Tag;
        }

        private void SceneTree_BeforeLabelEdit(object? sender, NodeLabelEditEventArgs args)
        {
            if (args.Node == _sceneTreeRoot) args.CancelEdit = true;
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

        private void EditEntity_ToolStripMenuItem_Clicked(object? sender, EventArgs args)
        {
            var e = (TransformEntity)scene_TreeView.SelectedNode.Tag;
            var editCompDialog = new EditComponentDialog(e);
            editCompDialog.ShowDialog();
        }

        private void AddEntity_ToolStripMenuItem_Click(object? sender, EventArgs args)
        {
            var parent = GetSelectedEntityInSceneTree();
            Editor.CreateNewEntity(parent);
        }

        private void RemoveEntity_ToolStripMenuItem_Click(object? sender, EventArgs args)
        {
            var e = GetSelectedEntityInSceneTree();
            Editor.RemoveEntity(e);
        }

        private TransformEntity GetSelectedEntityInSceneTree()
        {
            return (TransformEntity)scene_TreeView.SelectedNode.Tag;
        }

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
                if (scene_TreeView.Tag == currentScene) return;

                //
                // scene_treeview tag is current game scene
                // so we can unregisted old scene events
                //
                if (scene_TreeView.Tag != null)
                {
                    var oldScene = (GameScene)scene_TreeView.Tag;
                    oldScene.EntityAdded -= Scene_EntityAdded;
                    oldScene.EntityRemoved -= Scene_EntityRemoved;
                }

                //
                // Clean previous tree data
                //
                scene_TreeView.Tag = currentScene;
                scene_TreeView.Nodes.Clear();
                _entityToNode.Clear();
                _sceneTreeRoot = null;

                if (currentScene == null) return;

                currentScene.EntityAdded += Scene_EntityAdded;
                currentScene.EntityRemoved += Scene_EntityRemoved;

                //
                // init tree root
                //
                _sceneTreeRoot = scene_TreeView.Nodes.Add(currentScene.ToString());
                _sceneTreeRoot.ContextMenuStrip = sceneRoot_ContextMenuStrip;

                //
                // update new added entity, make sure parent alway get added first
                //
                var sortedEntities = TransformEntityHelper.SortByDepth(currentScene.Entities);
                foreach (var e in sortedEntities)
                {
                    AddSceneNode(e);
                }
            });
        }

        void Scene_EntityAdded(TransformEntity entity)
        {
            this.SafeInvoke(() => AddSceneNode(entity));
        }

        void Scene_EntityRemoved(TransformEntity entity)
        {
            RemoveSceneNode(entity);
        }

        private void AddSceneNode(TransformEntity e)
        {
            if (_entityToNode.ContainsKey(e)) return;

            var parentNode = _sceneTreeRoot;

            if (e.Parent == null)
            {
                // ignore
            }
            else if (_entityToNode.ContainsKey(e.Parent))
            {
                parentNode = _entityToNode[e.Parent];
            }
            else
            {
                throw new InvalidOperationException("Parent node have to be added before children");
            }


            var entityNode = parentNode.Nodes.Add(e.ToString());
            entityNode.ContextMenuStrip = sceneEntity_ContextMenuStrip;
            entityNode.Tag = e;

            _entityToNode[e] = entityNode;
        }

        private void RemoveSceneNode(TransformEntity e)
        {
            this.SafeInvoke(() =>
            {
                _entityToNode[e].Remove();
                _entityToNode.Remove(e);
            });

        }

        TreeNode? _sceneTreeRoot;
        Dictionary<TransformEntity, TreeNode> _entityToNode = [];

        internal GameEditor Editor
        {
            get => _editor ?? throw new NullReferenceException();
            set
            {
                if (_editor == value) return;

                if (_editor != null)
                {
                    _editor.SelectingEntityChanged -= EditorApp_SelectedEntityChanged;
                    _editor.CurrentSceneChanged -= EditorApp_CurrentSceneChanged;
                }

                _editor = value;

                _editor.SelectingEntityChanged += EditorApp_SelectedEntityChanged;
                _editor.CurrentSceneChanged += EditorApp_CurrentSceneChanged;

                UpdateSceneTree(_editor.SelectedEntity);
                UpdateSceneTree(_editor.CurrentGameScene);
            }
        }
        GameEditor? _editor;
    }
}

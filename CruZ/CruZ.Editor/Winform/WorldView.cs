using System;
using System.Collections.Generic;
using System.Windows.Forms;

using CruZ.Editor.Controls;
using CruZ.Editor.Winform.Utility;
using CruZ.GameEngine.GameSystem;

namespace CruZ.Editor
{
    public partial class WorldViewer : UserControl
    {
        public WorldViewer()
        {
            InitializeComponent();

            world_TreeView.HideSelection = false;
            world_TreeView.BeforeSelect += Tree_BeforeSelect;
            world_TreeView.NodeMouseClick += (sender, args) => world_TreeView.SelectedNode = args.Node;
            //world_TreeView.BeforeLabelEdit += SceneTree_BeforeLabelEdit;
            //world_TreeView.AfterLabelEdit += SceneTree_AfterLabelEdit;

            //addEntity_SceneRoot_ToolStripMenuItem.Click += AddEntity_ToolStripMenuItem_Click;
            //addEntity_RootChild_ToolStripMenuItem.Click += AddEntity_ToolStripMenuItem_Click;
            //editEntity_ToolStripMenuItem.Click += EditEntity_ToolStripMenuItem_Clicked;
            //removeEntity_ToolStripMenuItem.Click += RemoveEntity_ToolStripMenuItem_Click;

            ECSManager.InstanceChanged += ECSManager_InstanceChanged;
        }

        private void Tree_BeforeSelect(object? sender, TreeViewCancelEventArgs e)
        {
            Editor.SelectedEntity = (TransformEntity)e.Node.Tag;
        }

        //private void SceneTree_BeforeLabelEdit(object? sender, NodeLabelEditEventArgs args)
        //{
        //    if (args.Node == _treeRoot) args.CancelEdit = true;
        //}

        //private void SceneTree_AfterLabelEdit(object? sender, NodeLabelEditEventArgs args)
        //{
        //    if (string.IsNullOrEmpty(args.Label))
        //    {
        //        return;
        //    }

        //    var e = (TransformEntity)args.Node.Tag;
        //    e.Name = args.Label;
        //    InvalidateService.Invalidate(InvalidatedEvents.EntityNameChanged);
        //}

        //private void EditEntity_ToolStripMenuItem_Clicked(object? sender, EventArgs args)
        //{
        //    var e = (TransformEntity)world_TreeView.SelectedNode.Tag;
        //    var editCompDialog = new EditComponentDialog(e);
        //    editCompDialog.ShowDialog();
        //}

        //private void AddEntity_ToolStripMenuItem_Click(object? sender, EventArgs args)
        //{
        //    var parent = GetSelectedEntityInSceneTree();
        //    Editor.CreateNewEntity(parent);
        //}

        //private void RemoveEntity_ToolStripMenuItem_Click(object? sender, EventArgs args)
        //{
        //    var e = GetSelectedEntityInSceneTree();
        //    Editor.RemoveEntity(e);
        //}

        //private TransformEntity GetSelectedEntityInSceneTree()
        //{
        //    return (TransformEntity)world_TreeView.SelectedNode.Tag;
        //}

        private void ECSManager_InstanceChanged(ECSManager? oldECS, ECSManager ecs)
        {
            InitTree(oldECS?.World, ecs.World);
        }

        private void EditorApp_SelectedEntityChanged(TransformEntity? e)
        {
            UpdateTree(e);
        }

        private void UpdateTree(TransformEntity? selectEntity)
        {
            world_TreeView.SafeInvoke(delegate
            {
                world_TreeView.SelectedNode = selectEntity == null ? null : _entityToNode[selectEntity];
            });
        }

        private void InitTree(ECSWorld? oldWorld, ECSWorld world)
        {
            world_TreeView.SafeInvoke(delegate
            {
                
                if (world_TreeView.Tag == world) return;
                //
                // unregister event from old instnace
                //
                if (oldWorld != null)
                {
                    oldWorld.EntityAdded -= OnEntityAdded;
                    oldWorld.EntityRemoved -= OnEntityRemoved;
                }
                //
                // Clean previous tree data
                //
                world_TreeView.Tag = world;
                world_TreeView.Nodes.Clear();
                _entityToNode.Clear();
                //
                // init data with new ECSWorld
                //
                world.EntityAdded += OnEntityAdded;
                world.EntityRemoved += OnEntityRemoved;
                //
                // init tree root
                //
                //_treeRoot.ContextMenuStrip = world_ContextMenuStrip;
                //
                // update new added entity, make sure parent alway get added first
                //
                var sortedEntities = TransformEntityHelper.SortByDepth(world.Entities);
                foreach (var e in sortedEntities)
                {
                    AddTreeNode(e);
                }
            });
        }

        void OnEntityAdded(TransformEntity entity)
        {
            this.SafeInvoke(() => AddTreeNode(entity));
        }

        void OnEntityRemoved(TransformEntity entity)
        {
            RemoveTreeNode(entity);
        }

        /// <summary>
        /// Add node to tree view
        /// </summary>
        private void AddTreeNode(TransformEntity e)
        {
            if (_entityToNode.ContainsKey(e)) return;

            TreeNode entityNode;

            if (e.Parent == null)
            {
                entityNode = world_TreeView.Nodes.Add(e.ToString());
            }
            else
            {
                var parentNode = _entityToNode[e.Parent];
                entityNode = parentNode.Nodes.Add(e.ToString());
            }

            entityNode.Tag = e;
            _entityToNode[e] = entityNode;
        }

        private void RemoveTreeNode(TransformEntity e)
        {
            this.SafeInvoke(() =>
            {
                _entityToNode[e].Remove();
                _entityToNode.Remove(e);
            });

        }

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
                }

                _editor = value;

                _editor.SelectingEntityChanged += EditorApp_SelectedEntityChanged;

                UpdateTree(_editor.SelectedEntity);
            }
        }
        GameEditor? _editor;
    }
}

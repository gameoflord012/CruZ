using System;
using System.Collections.Generic;
using System.Windows.Forms;

using CruZ.Editor.Controls;
using CruZ.Editor.Winform.Utility;
using CruZ.GameEngine.GameSystem;

namespace CruZ.Editor
{
    public partial class SceneViewer : UserControl
    {
        public SceneViewer()
        {
            _entityToNode = [];

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

        private void UpdateTree(TransformEntity? selectEntity)
        {
            world_TreeView.SafeInvoke(delegate
            {
                world_TreeView.SelectedNode = selectEntity == null ? null : _entityToNode[selectEntity];
            });
        }

        private void InitTree(ECSWorld? oldWorld, ECSWorld? newWorld)
        {
            world_TreeView.SafeInvoke(delegate
            {
                world_TreeView.Nodes.Clear();
                _entityToNode.Clear();

                if(oldWorld != null)
                {
                    oldWorld.EntityAdded -= OnEntityAdded;
                    oldWorld.EntityRemoved -= OnEntityRemoved;
                }

                if(newWorld != null)
                {
                    newWorld.EntityAdded += OnEntityAdded;
                    newWorld.EntityRemoved += OnEntityRemoved;

                    // update new added entity, make sure parent alway get added first
                    var sortedEntities = TransformEntityHelper.SortByDepth(newWorld.Entities);
                    foreach(var e in sortedEntities)
                    {
                        AddTreeNode(e);
                    }
                }
            });
        }

        private void AddTreeNode(TransformEntity e)
        {
            if(_entityToNode.ContainsKey(e)) return;

            TreeNode entityNode;

            if(e.Parent == null)
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

        private void OnEntityAdded(TransformEntity entity)
        {
            this.SafeInvoke(() => AddTreeNode(entity));
        }

        private void OnEntityRemoved(TransformEntity entity)
        {
            RemoveTreeNode(entity);
        }

        private void Tree_BeforeSelect(object? sender, TreeViewCancelEventArgs e)
        {
            Editor.SelectedEntity = (TransformEntity)e.Node.Tag;
        }

        private void ECSManager_InstanceChanged(ECSManager? oldECS, ECSManager? ecs)
        {
            InitTree(oldECS?.World, ecs?.World);
        }

        private void EditorApp_SelectedEntityChanged(TransformEntity? e)
        {
            UpdateTree(e);
        }

        internal GameEditor Editor
        {
            get => _editor ?? throw new NullReferenceException();
            set
            {
                if(_editor == value) return;

                if(_editor != null)
                {
                    _editor.SelectingEntityChanged -= EditorApp_SelectedEntityChanged;
                }

                _editor = value;

                _editor.SelectingEntityChanged += EditorApp_SelectedEntityChanged;

                UpdateTree(_editor.SelectedEntity);
            }
        }

        private GameEditor? _editor;
        private Dictionary<TransformEntity, TreeNode> _entityToNode;
    }
}

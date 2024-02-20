using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CruZ.Components;
using CruZ.Editor.Controls;
using CruZ.Editor.Utility;

namespace CruZ.Editor
{
    public partial class SceneEditor : UserControl
    {
        public SceneEditor()
        {
            InitializeComponent();

            scene_TreeView.HideSelection = false;
            scene_TreeView.BeforeSelect += SceneTree_BeforeSelect;
            scene_TreeView.ContextMenuStrip = sceneEntity_ContextMenuStrip;
            scene_TreeView.NodeMouseClick += (sender, args)
                => scene_TreeView.SelectedNode = args.Node;

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

        #region Event Handlers
        private void SceneTree_BeforeSelect(object? sender, TreeViewCancelEventArgs e)
        {
            _editor.SelectedEntity = (TransformEntity)e.Node.Tag;
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

                if (currentScene == null) return;

                scene_TreeView.Nodes.Add(currentScene.ToString());
                var root = scene_TreeView.Nodes[0];
                root.ContextMenuStrip = sceneEntity_ContextMenuStrip;

                for (int i = 0; i < currentScene.Entities.Count(); i++)
                {
                    var e = currentScene.Entities[i];
                    root.Nodes.Add(e.ToString());

                    var entity_Node = root.Nodes[i];

                    entity_Node.ContextMenuStrip = sceneEntity_ContextMenuStrip;
                    entity_Node.Tag = e;

                    _entityToNode[e] = entity_Node;
                }
            });
        } 
        #endregion

        GameEditor _editor;
        Dictionary<TransformEntity, TreeNode> _entityToNode = [];
    }
}

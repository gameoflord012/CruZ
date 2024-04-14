using System.Drawing;

namespace CruZ.Editor
{
    partial class SceneEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private global::System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            sceneEntity_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            removeEntity_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            editEntity_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            scene_TreeView = new System.Windows.Forms.TreeView();
            sceneEditor_Panel = new System.Windows.Forms.Panel();
            sceneRoot_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            addEntity_SceneRoot_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addEntity_RootChild_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            sceneEntity_ContextMenuStrip.SuspendLayout();
            sceneEditor_Panel.SuspendLayout();
            sceneRoot_ContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // sceneEntity_ContextMenuStrip
            // 
            sceneEntity_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addEntity_RootChild_ToolStripMenuItem, removeEntity_ToolStripMenuItem, editEntity_ToolStripMenuItem });
            sceneEntity_ContextMenuStrip.Name = "scene_ContextMenuStrip";
            sceneEntity_ContextMenuStrip.Size = new Size(162, 70);
            // 
            // removeEntity_ToolStripMenuItem
            // 
            removeEntity_ToolStripMenuItem.Name = "removeEntity_ToolStripMenuItem";
            removeEntity_ToolStripMenuItem.Size = new Size(161, 22);
            removeEntity_ToolStripMenuItem.Text = "Remove Entity";
            // 
            // editEntity_ToolStripMenuItem
            // 
            editEntity_ToolStripMenuItem.Name = "editEntity_ToolStripMenuItem";
            editEntity_ToolStripMenuItem.Size = new Size(161, 22);
            editEntity_ToolStripMenuItem.Text = "Edit Component";
            // 
            // scene_TreeView
            // 
            scene_TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            scene_TreeView.Location = new Point(0, 0);
            scene_TreeView.Name = "scene_TreeView";
            scene_TreeView.Size = new Size(331, 196);
            scene_TreeView.TabIndex = 1;
            // 
            // sceneEditor_Panel
            // 
            sceneEditor_Panel.Controls.Add(scene_TreeView);
            sceneEditor_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            sceneEditor_Panel.Location = new Point(0, 0);
            sceneEditor_Panel.Name = "sceneEditor_Panel";
            sceneEditor_Panel.Size = new Size(331, 196);
            sceneEditor_Panel.TabIndex = 2;
            // 
            // sceneRoot_ContextMenuStrip
            // 
            sceneRoot_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addEntity_SceneRoot_ToolStripMenuItem });
            sceneRoot_ContextMenuStrip.Name = "scene_ContextMenuStrip";
            sceneRoot_ContextMenuStrip.Size = new Size(130, 26);
            // 
            // addEntity_SceneRoot_ToolStripMenuItem
            // 
            addEntity_SceneRoot_ToolStripMenuItem.Name = "addEntity_SceneRoot_ToolStripMenuItem";
            addEntity_SceneRoot_ToolStripMenuItem.Size = new Size(129, 22);
            addEntity_SceneRoot_ToolStripMenuItem.Text = "Add Entity";
            // 
            // addEntity_RootChild_ToolStripMenuItem
            // 
            addEntity_RootChild_ToolStripMenuItem.Name = "addEntity_RootChild_ToolStripMenuItem";
            addEntity_RootChild_ToolStripMenuItem.Size = new Size(161, 22);
            addEntity_RootChild_ToolStripMenuItem.Text = "Add Entity";
            // 
            // SceneEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(sceneEditor_Panel);
            Name = "SceneEditor";
            Size = new Size(331, 196);
            sceneEntity_ContextMenuStrip.ResumeLayout(false);
            sceneEditor_Panel.ResumeLayout(false);
            sceneRoot_ContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private global::System.Windows.Forms.ContextMenuStrip sceneEntity_ContextMenuStrip;
        private global::System.Windows.Forms.ToolStripMenuItem editEntity_ToolStripMenuItem;
        private global::System.Windows.Forms.TreeView scene_TreeView;
        private global::System.Windows.Forms.Panel sceneEditor_Panel;
        private global::System.Windows.Forms.ToolStripMenuItem addEntity_SceneRoot_ToolStripMenuItem;
        private global::System.Windows.Forms.ContextMenuStrip sceneRoot_ContextMenuStrip;
        private global::System.Windows.Forms.ToolStripMenuItem removeEntity_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addEntity_RootChild_ToolStripMenuItem;
    }
}

namespace CruZ.Editor
{
    partial class SceneEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            editEntity_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            scene_TreeView = new System.Windows.Forms.TreeView();
            sceneEditor_Panel = new System.Windows.Forms.Panel();
            sceneRoot_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            addEntity_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            removeEntity_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            sceneEntity_ContextMenuStrip.SuspendLayout();
            sceneEditor_Panel.SuspendLayout();
            sceneRoot_ContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // sceneEntity_ContextMenuStrip
            // 
            sceneEntity_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { removeEntity_ToolStripMenuItem, editEntity_ToolStripMenuItem });
            sceneEntity_ContextMenuStrip.Name = "scene_ContextMenuStrip";
            sceneEntity_ContextMenuStrip.Size = new DRAW.Size(181, 70);
            // 
            // editEntity_ToolStripMenuItem
            // 
            editEntity_ToolStripMenuItem.Name = "editEntity_ToolStripMenuItem";
            editEntity_ToolStripMenuItem.Size = new DRAW.Size(180, 22);
            editEntity_ToolStripMenuItem.Text = "Edit Component";
            // 
            // scene_TreeView
            // 
            scene_TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            scene_TreeView.Location = new DRAW.Point(0, 0);
            scene_TreeView.Name = "scene_TreeView";
            scene_TreeView.Size = new DRAW.Size(331, 196);
            scene_TreeView.TabIndex = 1;
            // 
            // sceneEditor_Panel
            // 
            sceneEditor_Panel.Controls.Add(scene_TreeView);
            sceneEditor_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            sceneEditor_Panel.Location = new DRAW.Point(0, 0);
            sceneEditor_Panel.Name = "sceneEditor_Panel";
            sceneEditor_Panel.Size = new DRAW.Size(331, 196);
            sceneEditor_Panel.TabIndex = 2;
            // 
            // sceneRoot_ContextMenuStrip
            // 
            sceneRoot_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addEntity_ToolStripMenuItem });
            sceneRoot_ContextMenuStrip.Name = "scene_ContextMenuStrip";
            sceneRoot_ContextMenuStrip.Size = new DRAW.Size(130, 26);
            // 
            // addEntity_ToolStripMenuItem
            // 
            addEntity_ToolStripMenuItem.Name = "addEntity_ToolStripMenuItem";
            addEntity_ToolStripMenuItem.Size = new DRAW.Size(129, 22);
            addEntity_ToolStripMenuItem.Text = "Add Entity";
            // 
            // removeEntity_ToolStripMenuItem
            // 
            removeEntity_ToolStripMenuItem.Name = "removeEntity_ToolStripMenuItem";
            removeEntity_ToolStripMenuItem.Size = new DRAW.Size(180, 22);
            removeEntity_ToolStripMenuItem.Text = "Remove Entity";
            // 
            // SceneEditor
            // 
            AutoScaleDimensions = new DRAW.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(sceneEditor_Panel);
            Name = "SceneEditor";
            Size = new DRAW.Size(331, 196);
            sceneEntity_ContextMenuStrip.ResumeLayout(false);
            sceneEditor_Panel.ResumeLayout(false);
            sceneRoot_ContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip sceneEntity_ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editEntity_ToolStripMenuItem;
        private System.Windows.Forms.TreeView scene_TreeView;
        private System.Windows.Forms.Panel sceneEditor_Panel;
        private System.Windows.Forms.ToolStripMenuItem addEntity_ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip sceneRoot_ContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeEntity_ToolStripMenuItem;
    }
}

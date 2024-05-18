using System.Drawing;

namespace CruZ.Editor
{
    partial class SceneViewer
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
            world_TreeView = new System.Windows.Forms.TreeView();
            treeView_Panel = new System.Windows.Forms.Panel();
            treeView_Panel.SuspendLayout();
            SuspendLayout();
            // 
            // world_TreeView
            // 
            world_TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            world_TreeView.Location = new Point(0, 0);
            world_TreeView.Name = "world_TreeView";
            world_TreeView.Size = new Size(331, 196);
            world_TreeView.TabIndex = 1;
            // 
            // treeView_Panel
            // 
            treeView_Panel.Controls.Add(world_TreeView);
            treeView_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView_Panel.Location = new Point(0, 0);
            treeView_Panel.Name = "treeView_Panel";
            treeView_Panel.Size = new Size(331, 196);
            treeView_Panel.TabIndex = 2;
            // 
            // SceneViewer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(treeView_Panel);
            Name = "SceneEditor";
            Size = new Size(331, 196);
            treeView_Panel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private global::System.Windows.Forms.TreeView world_TreeView;
        private global::System.Windows.Forms.Panel treeView_Panel;
    }
}

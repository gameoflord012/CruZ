using MonoGame.Forms.NET.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace CruZ.Editor
{
    partial class EditorForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            worldViewControl = new WorldViewControl();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openSceneToolStripMenuItem = new ToolStripMenuItem();
            saveSceneToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 28);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(worldViewControl);
            splitContainer1.Size = new Size(800, 422);
            splitContainer1.SplitterDistance = 481;
            splitContainer1.TabIndex = 0;
            // 
            // worldViewControl
            // 
            worldViewControl.BackColor = Color.LightSeaGreen;
            worldViewControl.Dock = DockStyle.Fill;
            worldViewControl.Location = new Point(0, 0);
            worldViewControl.Name = "worldViewControl";
            worldViewControl.Size = new Size(481, 422);
            worldViewControl.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openSceneToolStripMenuItem, saveSceneToolStripMenuItem, saveAsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            fileToolStripMenuItem.Click += fileToolStripMenuItem_Click;
            // 
            // openSceneToolStripMenuItem
            // 
            openSceneToolStripMenuItem.Name = "openSceneToolStripMenuItem";
            openSceneToolStripMenuItem.Size = new Size(224, 26);
            openSceneToolStripMenuItem.Text = "Open Scene";
            openSceneToolStripMenuItem.Click += openSceneToolStripMenuItem_Click;
            // 
            // saveSceneToolStripMenuItem
            // 
            saveSceneToolStripMenuItem.Name = "saveSceneToolStripMenuItem";
            saveSceneToolStripMenuItem.Size = new Size(224, 26);
            saveSceneToolStripMenuItem.Text = "Save Scene";
            saveSceneToolStripMenuItem.Click += saveSceneToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(224, 26);
            saveAsToolStripMenuItem.Text = "Save As ...";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "EditorForm";
            Text = "EditorForm";
            splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer1;
        private WorldViewControl worldViewControl;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openSceneToolStripMenuItem;
        private ToolStripMenuItem saveSceneToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
    }
}
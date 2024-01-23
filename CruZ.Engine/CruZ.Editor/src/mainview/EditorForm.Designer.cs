using CruZ.Editor.Controls;
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
            inspectorPanel = new FlowLayoutPanel();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            sceneToolStripMenuItem = new ToolStripMenuItem();
            openSceneToolStripMenuItem = new ToolStripMenuItem();
            saveSceneToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            loadSceneToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Margin = new Padding(3, 2, 3, 2);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(worldViewControl);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(inspectorPanel);
            splitContainer1.Size = new Size(700, 314);
            splitContainer1.SplitterDistance = 419;
            splitContainer1.TabIndex = 0;
            // 
            // worldViewControl
            // 
            worldViewControl.BackColor = Color.LightSeaGreen;
            worldViewControl.Dock = DockStyle.Fill;
            worldViewControl.Location = new Point(0, 0);
            worldViewControl.Margin = new Padding(3, 2, 3, 2);
            worldViewControl.Name = "worldViewControl";
            worldViewControl.Size = new Size(419, 314);
            worldViewControl.TabIndex = 0;
            // 
            // inspectorPanel
            // 
            inspectorPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            inspectorPanel.AutoScroll = true;
            inspectorPanel.FlowDirection = FlowDirection.TopDown;
            inspectorPanel.Location = new Point(2, 0);
            inspectorPanel.Name = "inspectorPanel";
            inspectorPanel.Size = new Size(275, 314);
            inspectorPanel.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.Control;
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(700, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { sceneToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            fileToolStripMenuItem.Click += fileToolStripMenuItem_Click;
            // 
            // sceneToolStripMenuItem
            // 
            sceneToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openSceneToolStripMenuItem, saveSceneToolStripMenuItem, saveAsToolStripMenuItem, loadSceneToolStripMenuItem });
            sceneToolStripMenuItem.Name = "sceneToolStripMenuItem";
            sceneToolStripMenuItem.Size = new Size(180, 22);
            sceneToolStripMenuItem.Text = "Scene";
            // 
            // openSceneToolStripMenuItem
            // 
            openSceneToolStripMenuItem.Name = "openSceneToolStripMenuItem";
            openSceneToolStripMenuItem.Size = new Size(180, 22);
            openSceneToolStripMenuItem.Text = "Open Scene";
            openSceneToolStripMenuItem.Click += openSceneToolStripMenuItem_Click;
            // 
            // saveSceneToolStripMenuItem
            // 
            saveSceneToolStripMenuItem.Name = "saveSceneToolStripMenuItem";
            saveSceneToolStripMenuItem.Size = new Size(180, 22);
            saveSceneToolStripMenuItem.Text = "Save Scene";
            saveSceneToolStripMenuItem.Click += saveSceneToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(180, 22);
            saveAsToolStripMenuItem.Text = "Save As ...";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // loadSceneToolStripMenuItem
            // 
            loadSceneToolStripMenuItem.Name = "loadSceneToolStripMenuItem";
            loadSceneToolStripMenuItem.Size = new Size(180, 22);
            loadSceneToolStripMenuItem.Text = "Load Scene";
            loadSceneToolStripMenuItem.Click += loadSceneToolStripMenuItem_Click;
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 2, 3, 2);
            Name = "EditorForm";
            Text = "EditorForm";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
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
        private FlowLayoutPanel inspectorPanel;
        private ToolStripMenuItem sceneToolStripMenuItem;
        private ToolStripMenuItem saveSceneToolStripMenuItem;
        private ToolStripMenuItem loadSceneToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem openSceneToolStripMenuItem;
    }
}
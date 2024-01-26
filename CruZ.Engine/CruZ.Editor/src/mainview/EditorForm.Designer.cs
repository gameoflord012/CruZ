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
            inspector_PropertyGrid = new PropertyGrid();
            menuStrip = new MenuStrip();
            file_Menu = new ToolStripMenuItem();
            scene_Menu = new ToolStripMenuItem();
            openScene_MenuButton = new ToolStripMenuItem();
            saveScene_MenuButton = new ToolStripMenuItem();
            saveAsScene_MenuButton = new ToolStripMenuItem();
            loadScene_MenuButton = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
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
            splitContainer1.Panel2.Controls.Add(inspector_PropertyGrid);
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
            // inspector_PropertyGrid
            // 
            inspector_PropertyGrid.Dock = DockStyle.Fill;
            inspector_PropertyGrid.Location = new Point(0, 0);
            inspector_PropertyGrid.Name = "inspector_PropertyGrid";
            inspector_PropertyGrid.Size = new Size(277, 314);
            inspector_PropertyGrid.TabIndex = 0;
            // 
            // menuStrip
            // 
            menuStrip.BackColor = SystemColors.Control;
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { file_Menu });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(5, 2, 0, 2);
            menuStrip.Size = new Size(700, 24);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // file_Menu
            // 
            file_Menu.DropDownItems.AddRange(new ToolStripItem[] { scene_Menu });
            file_Menu.Name = "file_Menu";
            file_Menu.Size = new Size(37, 20);
            file_Menu.Text = "File";
            file_Menu.Click += File_Menu_Clicked;
            // 
            // scene_Menu
            // 
            scene_Menu.DropDownItems.AddRange(new ToolStripItem[] { openScene_MenuButton, saveScene_MenuButton, saveAsScene_MenuButton, loadScene_MenuButton });
            scene_Menu.Name = "scene_Menu";
            scene_Menu.Size = new Size(180, 22);
            scene_Menu.Text = "Scene";
            // 
            // openScene_MenuButton
            // 
            openScene_MenuButton.Name = "openScene_MenuButton";
            openScene_MenuButton.Size = new Size(180, 22);
            openScene_MenuButton.Text = "Open Scene";
            openScene_MenuButton.Click += OpenScene_Clicked;
            // 
            // saveScene_MenuButton
            // 
            saveScene_MenuButton.Name = "saveScene_MenuButton";
            saveScene_MenuButton.Size = new Size(180, 22);
            saveScene_MenuButton.Text = "Save Scene";
            saveScene_MenuButton.Click += SaveScene_Clicked;
            // 
            // saveAsScene_MenuButton
            // 
            saveAsScene_MenuButton.Name = "saveAsScene_MenuButton";
            saveAsScene_MenuButton.Size = new Size(180, 22);
            saveAsScene_MenuButton.Text = "Save As ...";
            saveAsScene_MenuButton.Click += SaveAsScene_Clicked;
            // 
            // loadScene_MenuButton
            // 
            loadScene_MenuButton.Name = "loadScene_MenuButton";
            loadScene_MenuButton.Size = new Size(180, 22);
            loadScene_MenuButton.Text = "Load Scene";
            loadScene_MenuButton.Click += LoadScene_Clicked;
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new Padding(3, 2, 3, 2);
            Name = "EditorForm";
            Text = "EditorForm";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer1;
        private WorldViewControl worldViewControl;
        private MenuStrip menuStrip;
        private ToolStripMenuItem file_Menu;
        private ToolStripMenuItem scene_Menu;
        private ToolStripMenuItem saveScene_MenuButton;
        private ToolStripMenuItem loadScene_MenuButton;
        private ToolStripMenuItem saveAsScene_MenuButton;
        private ToolStripMenuItem openScene_MenuButton;
        private PropertyGrid inspector_PropertyGrid;
    }
}
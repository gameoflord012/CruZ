using CruZ.Editor.Controls;
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
        /// the contents of this method with the code _editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            file_Menu = new ToolStripMenuItem();
            scene_Menu = new ToolStripMenuItem();
            openScene_MenuButton = new ToolStripMenuItem();
            saveScene_MenuButton = new ToolStripMenuItem();
            saveAsScene_MenuButton = new ToolStripMenuItem();
            loadScene_MenuButton = new ToolStripMenuItem();
            menuStrip = new MenuStrip();
            inspectorTab = new TabPage();
            entityInspector = new src.winform.EditorInspector();
            sceneTab = new TabPage();
            sceneTree = new TreeView();
            splitContainer1 = new SplitContainer();
            tabControlTop = new TabControl();
            tabControlBottom = new TabControl();
            entity_ContextMenuStrip = new ContextMenuStrip(components);
            componentEditor_ToolStripMenuItem = new ToolStripMenuItem();
            scene_ContextMenuStrip = new ContextMenuStrip(components);
            addEntity_ToolStripMenuItem = new ToolStripMenuItem();
            menuStrip.SuspendLayout();
            inspectorTab.SuspendLayout();
            sceneTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControlTop.SuspendLayout();
            tabControlBottom.SuspendLayout();
            entity_ContextMenuStrip.SuspendLayout();
            scene_ContextMenuStrip.SuspendLayout();
            SuspendLayout();
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
            scene_Menu.Size = new Size(105, 22);
            scene_Menu.Text = "Scene";
            // 
            // openScene_MenuButton
            // 
            openScene_MenuButton.Name = "openScene_MenuButton";
            openScene_MenuButton.Size = new Size(137, 22);
            openScene_MenuButton.Text = "Open Scene";
            openScene_MenuButton.Click += OpenScene_Clicked;
            // 
            // saveScene_MenuButton
            // 
            saveScene_MenuButton.Name = "saveScene_MenuButton";
            saveScene_MenuButton.Size = new Size(137, 22);
            saveScene_MenuButton.Text = "Save Scene";
            saveScene_MenuButton.Click += SaveScene_Clicked;
            // 
            // saveAsScene_MenuButton
            // 
            saveAsScene_MenuButton.Name = "saveAsScene_MenuButton";
            saveAsScene_MenuButton.Size = new Size(137, 22);
            saveAsScene_MenuButton.Text = "Save As ...";
            saveAsScene_MenuButton.Click += SaveAsScene_Clicked;
            // 
            // loadScene_MenuButton
            // 
            loadScene_MenuButton.Name = "loadScene_MenuButton";
            loadScene_MenuButton.Size = new Size(137, 22);
            loadScene_MenuButton.Text = "Load Scene";
            loadScene_MenuButton.Click += LoadScene_Clicked;
            // 
            // menuStrip
            // 
            menuStrip.BackColor = SystemColors.ButtonHighlight;
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { file_Menu });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(5, 2, 0, 2);
            menuStrip.Size = new Size(377, 24);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // inspectorTab
            // 
            inspectorTab.Controls.Add(entityInspector);
            inspectorTab.Location = new Point(4, 24);
            inspectorTab.Name = "inspectorTab";
            inspectorTab.Padding = new Padding(3);
            inspectorTab.Size = new Size(369, 228);
            inspectorTab.TabIndex = 0;
            inspectorTab.Text = "Inspector";
            inspectorTab.UseVisualStyleBackColor = true;
            // 
            // entityInspector
            // 
            entityInspector.Dock = DockStyle.Fill;
            entityInspector.Location = new Point(3, 3);
            entityInspector.Name = "entityInspector";
            entityInspector.Size = new Size(363, 222);
            entityInspector.TabIndex = 0;
            // 
            // sceneTab
            // 
            sceneTab.Controls.Add(sceneTree);
            sceneTab.Location = new Point(4, 24);
            sceneTab.Name = "sceneTab";
            sceneTab.Padding = new Padding(3);
            sceneTab.Size = new Size(369, 70);
            sceneTab.TabIndex = 1;
            sceneTab.Text = "Scene Hierarchy";
            sceneTab.UseVisualStyleBackColor = true;
            // 
            // sceneTree
            // 
            sceneTree.Dock = DockStyle.Fill;
            sceneTree.Location = new Point(3, 3);
            sceneTree.Name = "sceneTree";
            sceneTree.Size = new Size(363, 64);
            sceneTree.TabIndex = 0;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(0, 27);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tabControlTop);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tabControlBottom);
            splitContainer1.Size = new Size(377, 358);
            splitContainer1.SplitterDistance = 98;
            splitContainer1.TabIndex = 5;
            // 
            // tabControlTop
            // 
            tabControlTop.Controls.Add(sceneTab);
            tabControlTop.Dock = DockStyle.Fill;
            tabControlTop.Location = new Point(0, 0);
            tabControlTop.Name = "tabControlTop";
            tabControlTop.SelectedIndex = 0;
            tabControlTop.Size = new Size(377, 98);
            tabControlTop.TabIndex = 5;
            // 
            // tabControlBottom
            // 
            tabControlBottom.Controls.Add(inspectorTab);
            tabControlBottom.Dock = DockStyle.Fill;
            tabControlBottom.Location = new Point(0, 0);
            tabControlBottom.Name = "tabControlBottom";
            tabControlBottom.SelectedIndex = 0;
            tabControlBottom.Size = new Size(377, 256);
            tabControlBottom.TabIndex = 6;
            // 
            // entity_ContextMenuStrip
            // 
            entity_ContextMenuStrip.Items.AddRange(new ToolStripItem[] { componentEditor_ToolStripMenuItem });
            entity_ContextMenuStrip.Name = "sceneClickMenu";
            entity_ContextMenuStrip.Size = new Size(167, 26);
            // 
            // componentEditor_ToolStripMenuItem
            // 
            componentEditor_ToolStripMenuItem.Name = "componentEditor_ToolStripMenuItem";
            componentEditor_ToolStripMenuItem.Size = new Size(166, 22);
            componentEditor_ToolStripMenuItem.Text = "Edit Components";
            // 
            // scene_ContextMenuStrip
            // 
            scene_ContextMenuStrip.Items.AddRange(new ToolStripItem[] { addEntity_ToolStripMenuItem });
            scene_ContextMenuStrip.Name = "scene_ContextMenuStrip";
            scene_ContextMenuStrip.Size = new Size(157, 26);
            // 
            // addEntity_ToolStripMenuItem
            // 
            addEntity_ToolStripMenuItem.Name = "addEntity_ToolStripMenuItem";
            addEntity_ToolStripMenuItem.Size = new Size(156, 22);
            addEntity_ToolStripMenuItem.Text = "Add New Entity";
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(377, 385);
            Controls.Add(menuStrip);
            Controls.Add(splitContainer1);
            MainMenuStrip = menuStrip;
            Margin = new Padding(3, 2, 3, 2);
            Name = "EditorForm";
            Text = "EditorForm";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            inspectorTab.ResumeLayout(false);
            sceneTab.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControlTop.ResumeLayout(false);
            tabControlBottom.ResumeLayout(false);
            entity_ContextMenuStrip.ResumeLayout(false);
            scene_ContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        #region WORLDVIEW_COMMENT
        //TODO:
        //_editor = new EditorApplication();


        //splitContainer1.Panel1.Controls.Add(_editor);

        //_editor.BackColor = Color.LightSeaGreen;
        //_editor.Dock = DockStyle.Fill;
        //_editor.Location = new Point(0, 0);
        //_editor.Margin = new Padding(3, 2, 3, 2);
        //_editor.Name = "_editor";
        //_editor.Size = new Size(419, 314);
        //_editor.TabIndex = 0;
        #endregion

        private ToolStripMenuItem file_Menu;
        private ToolStripMenuItem scene_Menu;
        private ToolStripMenuItem openScene_MenuButton;
        private ToolStripMenuItem saveScene_MenuButton;
        private ToolStripMenuItem saveAsScene_MenuButton;
        private ToolStripMenuItem loadScene_MenuButton;
        private MenuStrip menuStrip;
        private TabPage inspectorTab;
        private TabPage sceneTab;
        private TreeView sceneTree;
        private SplitContainer splitContainer1;
        private TabControl tabControlTop;
        private TabControl tabControlBottom;
        private ContextMenuStrip entity_ContextMenuStrip;
        private ToolStripMenuItem componentEditor_ToolStripMenuItem;
        private ContextMenuStrip scene_ContextMenuStrip;
        private ToolStripMenuItem addEntity_ToolStripMenuItem;
        private src.winform.EditorInspector entityInspector;
    }
}
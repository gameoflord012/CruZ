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
            entities_ComboBox = new ComboBox();
            inspector_PropertyGrid = new PropertyGrid();
            file_Menu = new ToolStripMenuItem();
            scene_Menu = new ToolStripMenuItem();
            openScene_MenuButton = new ToolStripMenuItem();
            saveScene_MenuButton = new ToolStripMenuItem();
            saveAsScene_MenuButton = new ToolStripMenuItem();
            loadScene_MenuButton = new ToolStripMenuItem();
            menuStrip = new MenuStrip();
            entities_Text = new Label();
            inspector_Panel = new Panel();
            menuStrip.SuspendLayout();
            inspector_Panel.SuspendLayout();
            SuspendLayout();
            // 
            // entities_ComboBox
            // 
            entities_ComboBox.Dock = DockStyle.Fill;
            entities_ComboBox.FormattingEnabled = true;
            entities_ComboBox.Location = new Point(0, 17);
            entities_ComboBox.Name = "entities_ComboBox";
            entities_ComboBox.Size = new Size(700, 23);
            entities_ComboBox.TabIndex = 1;
            // 
            // inspector_PropertyGrid
            // 
            inspector_PropertyGrid.Dock = DockStyle.Bottom;
            inspector_PropertyGrid.Location = new Point(0, 53);
            inspector_PropertyGrid.Name = "inspector_PropertyGrid";
            inspector_PropertyGrid.Size = new Size(700, 261);
            inspector_PropertyGrid.TabIndex = 0;
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
            menuStrip.Size = new Size(700, 24);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // entities_Text
            // 
            entities_Text.Dock = DockStyle.Top;
            entities_Text.Location = new Point(0, 0);
            entities_Text.Name = "entities_Text";
            entities_Text.Size = new Size(700, 17);
            entities_Text.TabIndex = 2;
            entities_Text.Text = "Entities";
            // 
            // inspector_Panel
            // 
            inspector_Panel.Controls.Add(inspector_PropertyGrid);
            inspector_Panel.Controls.Add(entities_ComboBox);
            inspector_Panel.Controls.Add(entities_Text);
            inspector_Panel.Dock = DockStyle.Fill;
            inspector_Panel.Location = new Point(0, 24);
            inspector_Panel.Name = "inspector_Panel";
            inspector_Panel.Size = new Size(700, 314);
            inspector_Panel.TabIndex = 3;
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(inspector_Panel);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new Padding(3, 2, 3, 2);
            Name = "EditorForm";
            Text = "EditorForm";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            inspector_Panel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PropertyGrid inspector_PropertyGrid;
        private ComboBox entities_ComboBox;

        #region WORLDVIEW_COMMENT
        //TODO:
        //_worldViewControl = new WorldViewControl();


        //splitContainer1.Panel1.Controls.Add(_worldViewControl);

        //_worldViewControl.BackColor = Color.LightSeaGreen;
        //_worldViewControl.Dock = DockStyle.Fill;
        //_worldViewControl.Location = new Point(0, 0);
        //_worldViewControl.Margin = new Padding(3, 2, 3, 2);
        //_worldViewControl.Name = "_worldViewControl";
        //_worldViewControl.Size = new Size(419, 314);
        //_worldViewControl.TabIndex = 0;
        #endregion

        private ToolStripMenuItem file_Menu;
        private ToolStripMenuItem scene_Menu;
        private ToolStripMenuItem openScene_MenuButton;
        private ToolStripMenuItem saveScene_MenuButton;
        private ToolStripMenuItem saveAsScene_MenuButton;
        private ToolStripMenuItem loadScene_MenuButton;
        private MenuStrip menuStrip;
        private Label entities_Text;
        private Panel inspector_Panel;
    }
}
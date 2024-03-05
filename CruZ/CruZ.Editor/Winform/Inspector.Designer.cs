namespace CruZ.Editor
{
    partial class Inspector
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
        /// the contents of this method with the code _gameEditor.
        /// </summary>
        private void InitializeComponent()
        {
            entities_ComboBox = new global::System.Windows.Forms.ComboBox();
            entities_Label = new global::System.Windows.Forms.Label();
            inspector_PropertyGrid = new global::System.Windows.Forms.PropertyGrid();
            inspector_Panel = new global::System.Windows.Forms.Panel();
            inspector_Panel.SuspendLayout();
            SuspendLayout();
            // 
            // entities_ComboBox
            // 
            entities_ComboBox.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
            entities_ComboBox.FormattingEnabled = true;
            entities_ComboBox.Location = new DRAW.Point(0, 17);
            entities_ComboBox.Name = "entities_ComboBox";
            entities_ComboBox.Size = new DRAW.Size(288, 23);
            entities_ComboBox.TabIndex = 1;
            // 
            // entities_Label
            // 
            entities_Label.Location = new DRAW.Point(0, 0);
            entities_Label.Name = "entities_Label";
            entities_Label.Size = new DRAW.Size(334, 17);
            entities_Label.TabIndex = 2;
            entities_Label.Text = "Entities";
            // 
            // inspector_PropertyGrid
            // 
            inspector_PropertyGrid.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
            inspector_PropertyGrid.Location = new DRAW.Point(0, 46);
            inspector_PropertyGrid.Name = "inspector_PropertyGrid";
            inspector_PropertyGrid.Size = new DRAW.Size(290, 187);
            inspector_PropertyGrid.TabIndex = 0;
            // 
            // inspector_Panel
            // 
            inspector_Panel.Controls.Add(entities_ComboBox);
            inspector_Panel.Controls.Add(entities_Label);
            inspector_Panel.Controls.Add(inspector_PropertyGrid);
            inspector_Panel.Dock = global::System.Windows.Forms.DockStyle.Fill;
            inspector_Panel.Location = new DRAW.Point(0, 0);
            inspector_Panel.Name = "inspector_Panel";
            inspector_Panel.Size = new DRAW.Size(288, 233);
            inspector_Panel.TabIndex = 4;
            // 
            // Inspector
            // 
            AutoScaleDimensions = new DRAW.SizeF(7F, 15F);
            AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(inspector_Panel);
            Name = "EntityInspector";
            Size = new DRAW.Size(288, 233);
            inspector_Panel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private global::System.Windows.Forms.ComboBox entities_ComboBox;
        private global::System.Windows.Forms.Label entities_Label;
        private global::System.Windows.Forms.PropertyGrid inspector_PropertyGrid;
        private global::System.Windows.Forms.Panel inspector_Panel;
    }
}

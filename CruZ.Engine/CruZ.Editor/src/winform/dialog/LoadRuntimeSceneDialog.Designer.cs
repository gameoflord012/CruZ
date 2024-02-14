namespace CruZ.Editor
{
    partial class LoadRuntimeSceneDialog
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
            selectScene_ComboBox = new System.Windows.Forms.ComboBox();
            scene_Label = new System.Windows.Forms.Label();
            ok_Button = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // selectScene_ComboBox
            // 
            selectScene_ComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            selectScene_ComboBox.FormattingEnabled = true;
            selectScene_ComboBox.Location = new DRAW.Point(12, 27);
            selectScene_ComboBox.Name = "selectScene_ComboBox";
            selectScene_ComboBox.Size = new DRAW.Size(402, 23);
            selectScene_ComboBox.TabIndex = 0;
            // 
            // scene_Label
            // 
            scene_Label.AutoSize = true;
            scene_Label.Location = new DRAW.Point(12, 9);
            scene_Label.Name = "scene_Label";
            scene_Label.Size = new DRAW.Size(116, 15);
            scene_Label.TabIndex = 1;
            scene_Label.Text = "Select runtime scene";
            // 
            // ok_Button
            // 
            ok_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            ok_Button.Location = new DRAW.Point(339, 178);
            ok_Button.Name = "ok_Button";
            ok_Button.Size = new DRAW.Size(75, 23);
            ok_Button.TabIndex = 2;
            ok_Button.Text = "OK";
            ok_Button.UseVisualStyleBackColor = true;
            // 
            // LoadRuntimeSceneDialog
            // 
            AutoScaleDimensions = new DRAW.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new DRAW.Size(426, 213);
            Controls.Add(ok_Button);
            Controls.Add(selectScene_ComboBox);
            Controls.Add(scene_Label);
            Name = "LoadRuntimeSceneForm";
            Text = "LoadRuntimeSceneForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox selectScene_ComboBox;
        private System.Windows.Forms.Label scene_Label;
        private System.Windows.Forms.Button ok_Button;
    }
}
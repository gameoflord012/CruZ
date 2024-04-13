using System.Drawing;

namespace CruZ.Editor
{
    partial class LoadRuntimeSceneDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code _gameEditor.
        /// </summary>
        private void InitializeComponent()
        {
            selectScene_ComboBox = new global::System.Windows.Forms.ComboBox();
            scene_Label = new global::System.Windows.Forms.Label();
            ok_Button = new global::System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // selectScene_ComboBox
            // 
            selectScene_ComboBox.Anchor = global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right;
            selectScene_ComboBox.FormattingEnabled = true;
            selectScene_ComboBox.Location = new Point(12, 27);
            selectScene_ComboBox.Name = "selectScene_ComboBox";
            selectScene_ComboBox.Size = new Size(402, 23);
            selectScene_ComboBox.TabIndex = 0;
            // 
            // scene_Label
            // 
            scene_Label.AutoSize = true;
            scene_Label.Location = new Point(12, 9);
            scene_Label.Name = "scene_Label";
            scene_Label.Size = new Size(116, 15);
            scene_Label.TabIndex = 1;
            scene_Label.Text = "Select runtime scene";
            // 
            // ok_Button
            // 
            ok_Button.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right;
            ok_Button.Location = new Point(339, 178);
            ok_Button.Name = "ok_Button";
            ok_Button.Size = new Size(75, 23);
            ok_Button.TabIndex = 2;
            ok_Button.Text = "OK";
            ok_Button.UseVisualStyleBackColor = true;
            // 
            // LoadRuntimeSceneDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(426, 213);
            Controls.Add(ok_Button);
            Controls.Add(selectScene_ComboBox);
            Controls.Add(scene_Label);
            Name = "LoadRuntimeSceneForm";
            Text = "LoadRuntimeSceneForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private global::System.Windows.Forms.ComboBox selectScene_ComboBox;
        private global::System.Windows.Forms.Label scene_Label;
        private global::System.Windows.Forms.Button ok_Button;
    }
}
namespace CruZ.Editor
{
    partial class EditComponentDialog
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
            selectComponent_ComboBox = new System.Windows.Forms.ComboBox();
            ok_Button = new System.Windows.Forms.Button();
            component_Label = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // selectComponent_ComboBox
            // 
            selectComponent_ComboBox.FormattingEnabled = true;
            selectComponent_ComboBox.Location = new DRAW.Point(12, 27);
            selectComponent_ComboBox.Name = "selectComponent_ComboBox";
            selectComponent_ComboBox.Size = new DRAW.Size(248, 23);
            selectComponent_ComboBox.TabIndex = 0;
            // 
            // ok_Button
            // 
            ok_Button.Location = new DRAW.Point(185, 153);
            ok_Button.Name = "ok_Button";
            ok_Button.Size = new DRAW.Size(75, 23);
            ok_Button.TabIndex = 1;
            ok_Button.Text = "OK";
            ok_Button.UseVisualStyleBackColor = true;
            // 
            // component_Label
            // 
            component_Label.AutoSize = true;
            component_Label.Location = new DRAW.Point(12, 9);
            component_Label.Name = "component_Label";
            component_Label.Size = new DRAW.Size(76, 15);
            component_Label.TabIndex = 2;
            component_Label.Text = "Components";
            // 
            // EditComponentDialog
            // 
            AutoScaleDimensions = new DRAW.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new DRAW.Size(272, 188);
            Controls.Add(selectComponent_ComboBox);
            Controls.Add(component_Label);
            Controls.Add(ok_Button);
            Name = "EditComponentDialog";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox selectComponent_ComboBox;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Label component_Label;
    }
}
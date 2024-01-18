namespace CruZ.Editor.Controls
{
    partial class Vector3InspectorControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            zValue = new System.Windows.Forms.TextBox();
            yValue = new System.Windows.Forms.TextBox();
            zText = new System.Windows.Forms.TextBox();
            yText = new System.Windows.Forms.TextBox();
            propertyName_TextBox = new System.Windows.Forms.TextBox();
            xText = new System.Windows.Forms.TextBox();
            xValue = new System.Windows.Forms.TextBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.6666641F));
            tableLayoutPanel1.Controls.Add(zValue, 1, 3);
            tableLayoutPanel1.Controls.Add(yValue, 1, 2);
            tableLayoutPanel1.Controls.Add(zText, 0, 3);
            tableLayoutPanel1.Controls.Add(yText, 0, 2);
            tableLayoutPanel1.Controls.Add(propertyName_TextBox, 0, 0);
            tableLayoutPanel1.Controls.Add(xText, 0, 1);
            tableLayoutPanel1.Controls.Add(xValue, 1, 1);
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.Size = new System.Drawing.Size(266, 123);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // zValue
            // 
            zValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            zValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            zValue.Location = new System.Drawing.Point(91, 92);
            zValue.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            zValue.Name = "zValue";
            zValue.Size = new System.Drawing.Size(172, 23);
            zValue.TabIndex = 6;
            // 
            // yValue
            // 
            yValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            yValue.Location = new System.Drawing.Point(91, 62);
            yValue.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            yValue.Name = "yValue";
            yValue.Size = new System.Drawing.Size(172, 23);
            yValue.TabIndex = 5;
            // 
            // zText
            // 
            zText.BackColor = System.Drawing.SystemColors.Menu;
            zText.ForeColor = System.Drawing.SystemColors.MenuText;
            zText.Location = new System.Drawing.Point(3, 92);
            zText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            zText.Name = "zText";
            zText.ReadOnly = true;
            zText.Size = new System.Drawing.Size(82, 23);
            zText.TabIndex = 3;
            zText.Text = "Z";
            zText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // yText
            // 
            yText.BackColor = System.Drawing.SystemColors.Menu;
            yText.ForeColor = System.Drawing.SystemColors.MenuText;
            yText.Location = new System.Drawing.Point(3, 62);
            yText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            yText.Name = "yText";
            yText.ReadOnly = true;
            yText.Size = new System.Drawing.Size(82, 23);
            yText.TabIndex = 2;
            yText.Text = "Y";
            yText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // propertyName_TextBox
            // 
            propertyName_TextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            propertyName_TextBox.BackColor = System.Drawing.SystemColors.Menu;
            tableLayoutPanel1.SetColumnSpan(propertyName_TextBox, 2);
            propertyName_TextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            propertyName_TextBox.ForeColor = System.Drawing.SystemColors.MenuText;
            propertyName_TextBox.Location = new System.Drawing.Point(3, 2);
            propertyName_TextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            propertyName_TextBox.Name = "propertyName_TextBox";
            propertyName_TextBox.ReadOnly = true;
            propertyName_TextBox.Size = new System.Drawing.Size(260, 23);
            propertyName_TextBox.TabIndex = 0;
            propertyName_TextBox.Text = "PropertyName";
            // 
            // xText
            // 
            xText.BackColor = System.Drawing.SystemColors.Menu;
            xText.ForeColor = System.Drawing.SystemColors.MenuText;
            xText.Location = new System.Drawing.Point(3, 32);
            xText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            xText.Name = "xText";
            xText.ReadOnly = true;
            xText.Size = new System.Drawing.Size(82, 23);
            xText.TabIndex = 1;
            xText.Text = "X";
            xText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // xValue
            // 
            xValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            xValue.Location = new System.Drawing.Point(91, 32);
            xValue.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            xValue.Name = "xValue";
            xValue.Size = new System.Drawing.Size(172, 23);
            xValue.TabIndex = 4;
            // 
            // Vector3InspectorControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "Vector3InspectorControl";
            Size = new System.Drawing.Size(266, 123);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox propertyName_TextBox;
        private System.Windows.Forms.TextBox xText;
        private System.Windows.Forms.TextBox yText;
        private System.Windows.Forms.TextBox xValue;
        private System.Windows.Forms.TextBox yValue;
        private System.Windows.Forms.TextBox zValue;
        private System.Windows.Forms.TextBox zText;
    }
}

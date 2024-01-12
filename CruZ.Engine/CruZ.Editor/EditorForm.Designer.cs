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
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Size = new Size(800, 450);
            splitContainer1.SplitterDistance = 549;
            splitContainer1.TabIndex = 0;
            // 
            // EditorForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer1);
            Name = "EditorForm";
            Text = "EditorForm";
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            //
            // MonoGameControl
            //
            worldViewControl = new WorldViewControl();
            worldViewControl.Dock = DockStyle.Fill;
            worldViewControl.BackColor = Color.LightSeaGreen;

            splitContainer1.Panel1.Controls.Add(worldViewControl);
        }

        #endregion

        private SplitContainer splitContainer1;
        private WorldViewControl worldViewControl;
    }
}
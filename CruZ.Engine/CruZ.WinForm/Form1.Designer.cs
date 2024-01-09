
namespace CruZ.WinForm
{
    partial class Form1
    {
        
        private System.ComponentModel.IContainer components = null;


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
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            contextMenuStrip1 = new ContextMenuStrip(components);
            fsdfasdsdfToolStripMenuItem = new ToolStripMenuItem();
            toolStrip2 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            contextMenuStrip1.SuspendLayout();
            toolStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { fsdfasdsdfToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(147, 28);
            // 
            // fsdfasdsdfToolStripMenuItem
            // 
            fsdfasdsdfToolStripMenuItem.Name = "fsdfasdsdfToolStripMenuItem";
            fsdfasdsdfToolStripMenuItem.Size = new Size(146, 24);
            fsdfasdsdfToolStripMenuItem.Text = "fsdfasdsdf";
            // 
            // toolStrip2
            // 
            toolStrip2.ImageScalingSize = new Size(20, 20);
            toolStrip2.Items.AddRange(new ToolStripItem[] { toolStripLabel1 });
            toolStrip2.Location = new Point(0, 24);
            toolStrip2.Name = "toolStrip1";
            toolStrip2.Size = new Size(800, 25);
            toolStrip2.TabIndex = 2;
            toolStrip2.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(111, 22);
            toolStripLabel1.Text = "toolStripLabel1";
            toolStripLabel1.Click += toolStripLabel1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(toolStrip2);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            contextMenuStrip1.ResumeLayout(false);
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DrawTest _drawTest = new DrawTest();
        private MenuStrip menuStrip1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem fsdfasdsdfToolStripMenuItem;
        private ToolStrip toolStrip2;
        private ToolStripLabel toolStripLabel1;
    }
}
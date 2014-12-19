namespace Examples
{
    partial class ExampleBrowser
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
            this.treeViewExamples = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // treeViewExamples
            // 
            this.treeViewExamples.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewExamples.Location = new System.Drawing.Point(12, 25);
            this.treeViewExamples.Name = "treeViewExamples";
            this.treeViewExamples.Size = new System.Drawing.Size(344, 297);
            this.treeViewExamples.TabIndex = 1;
            this.treeViewExamples.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeViewExamples_NodeMouseDoubleClick);
            this.treeViewExamples.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TreeViewExamples_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Example projects: Double-click to run.";
            // 
            // ExampleBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 334);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeViewExamples);
            this.KeyPreview = true;
            this.Name = "ExampleBrowser";
            this.Text = "ObjectTK example projects";
            this.Load += new System.EventHandler(this.ExampleBrowser_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExampleBrowser_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewExamples;
        private System.Windows.Forms.Label label1;
    }
}
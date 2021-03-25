
namespace TransformationTesting
{
    partial class FoundFilesForm
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
            this.labFoundXml = new System.Windows.Forms.Label();
            this.labFoundXslt = new System.Windows.Forms.Label();
            this.lbXml = new System.Windows.Forms.ListBox();
            this.lbXslt = new System.Windows.Forms.ListBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labFoundXml
            // 
            this.labFoundXml.AutoSize = true;
            this.labFoundXml.Location = new System.Drawing.Point(119, 32);
            this.labFoundXml.Name = "labFoundXml";
            this.labFoundXml.Size = new System.Drawing.Size(62, 13);
            this.labFoundXml.TabIndex = 0;
            this.labFoundXml.Text = "Found XML";
            // 
            // labFoundXslt
            // 
            this.labFoundXslt.AutoSize = true;
            this.labFoundXslt.Location = new System.Drawing.Point(566, 32);
            this.labFoundXslt.Name = "labFoundXslt";
            this.labFoundXslt.Size = new System.Drawing.Size(67, 13);
            this.labFoundXslt.TabIndex = 1;
            this.labFoundXslt.Text = "Found XSLT";
            // 
            // lbXml
            // 
            this.lbXml.FormattingEnabled = true;
            this.lbXml.Location = new System.Drawing.Point(13, 70);
            this.lbXml.Name = "lbXml";
            this.lbXml.Size = new System.Drawing.Size(319, 329);
            this.lbXml.TabIndex = 2;
            this.lbXml.SelectedIndexChanged += new System.EventHandler(this.lbXml_SelectedIndexChanged);
            // 
            // lbXslt
            // 
            this.lbXslt.FormattingEnabled = true;
            this.lbXslt.Location = new System.Drawing.Point(427, 70);
            this.lbXslt.Name = "lbXslt";
            this.lbXslt.Size = new System.Drawing.Size(319, 329);
            this.lbXslt.TabIndex = 3;
            this.lbXslt.SelectedIndexChanged += new System.EventHandler(this.lbXslt_SelectedIndexChanged);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(338, 70);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FoundFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lbXslt);
            this.Controls.Add(this.lbXml);
            this.Controls.Add(this.labFoundXslt);
            this.Controls.Add(this.labFoundXml);
            this.Name = "FoundFilesForm";
            this.Text = "FoundFilesForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labFoundXml;
        private System.Windows.Forms.Label labFoundXslt;
        private System.Windows.Forms.ListBox lbXml;
        private System.Windows.Forms.ListBox lbXslt;
        private System.Windows.Forms.Button btnOk;
    }
}
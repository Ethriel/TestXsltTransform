
namespace TransformationTesting
{
    partial class TestXSLT
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
            this.labSelectXml = new System.Windows.Forms.Label();
            this.labSelectXSLT = new System.Windows.Forms.Label();
            this.btnSelectXML = new System.Windows.Forms.Button();
            this.btnSelectXSLT = new System.Windows.Forms.Button();
            this.labSelectedXML = new System.Windows.Forms.Label();
            this.labSelectedXSLT = new System.Windows.Forms.Label();
            this.btnTransform = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnTransformToJSON = new System.Windows.Forms.Button();
            this.labSelectedJSON = new System.Windows.Forms.Label();
            this.btnSelectJSON = new System.Windows.Forms.Button();
            this.labSelectJSON = new System.Windows.Forms.Label();
            this.btnJSONtoXML = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labSelectXml
            // 
            this.labSelectXml.AutoSize = true;
            this.labSelectXml.Location = new System.Drawing.Point(26, 37);
            this.labSelectXml.Name = "labSelectXml";
            this.labSelectXml.Size = new System.Drawing.Size(78, 13);
            this.labSelectXml.TabIndex = 0;
            this.labSelectXml.Text = "Select XML file";
            // 
            // labSelectXSLT
            // 
            this.labSelectXSLT.AutoSize = true;
            this.labSelectXSLT.Location = new System.Drawing.Point(26, 69);
            this.labSelectXSLT.Name = "labSelectXSLT";
            this.labSelectXSLT.Size = new System.Drawing.Size(83, 13);
            this.labSelectXSLT.TabIndex = 1;
            this.labSelectXSLT.Text = "Select XSLT file";
            // 
            // btnSelectXML
            // 
            this.btnSelectXML.Location = new System.Drawing.Point(145, 32);
            this.btnSelectXML.Name = "btnSelectXML";
            this.btnSelectXML.Size = new System.Drawing.Size(75, 23);
            this.btnSelectXML.TabIndex = 2;
            this.btnSelectXML.Text = "Choose...";
            this.btnSelectXML.UseVisualStyleBackColor = true;
            this.btnSelectXML.Click += new System.EventHandler(this.btnSelectXML_Click);
            // 
            // btnSelectXSLT
            // 
            this.btnSelectXSLT.Location = new System.Drawing.Point(145, 64);
            this.btnSelectXSLT.Name = "btnSelectXSLT";
            this.btnSelectXSLT.Size = new System.Drawing.Size(75, 23);
            this.btnSelectXSLT.TabIndex = 3;
            this.btnSelectXSLT.Text = "Choose...";
            this.btnSelectXSLT.UseVisualStyleBackColor = true;
            this.btnSelectXSLT.Click += new System.EventHandler(this.btnSelectXSLT_Click);
            // 
            // labSelectedXML
            // 
            this.labSelectedXML.AutoSize = true;
            this.labSelectedXML.Location = new System.Drawing.Point(226, 37);
            this.labSelectedXML.Name = "labSelectedXML";
            this.labSelectedXML.Size = new System.Drawing.Size(106, 13);
            this.labSelectedXML.TabIndex = 4;
            this.labSelectedXML.Text = "No XML was chosen";
            // 
            // labSelectedXSLT
            // 
            this.labSelectedXSLT.AutoSize = true;
            this.labSelectedXSLT.Location = new System.Drawing.Point(226, 69);
            this.labSelectedXSLT.Name = "labSelectedXSLT";
            this.labSelectedXSLT.Size = new System.Drawing.Size(111, 13);
            this.labSelectedXSLT.TabIndex = 5;
            this.labSelectedXSLT.Text = "No XSLT was chosen";
            // 
            // btnTransform
            // 
            this.btnTransform.Enabled = false;
            this.btnTransform.Location = new System.Drawing.Point(29, 122);
            this.btnTransform.Name = "btnTransform";
            this.btnTransform.Size = new System.Drawing.Size(110, 35);
            this.btnTransform.TabIndex = 6;
            this.btnTransform.Text = "XML with XSLT";
            this.btnTransform.UseVisualStyleBackColor = true;
            this.btnTransform.Click += new System.EventHandler(this.btnTransform_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(29, 167);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnTransformToJSON
            // 
            this.btnTransformToJSON.Location = new System.Drawing.Point(145, 122);
            this.btnTransformToJSON.Name = "btnTransformToJSON";
            this.btnTransformToJSON.Size = new System.Drawing.Size(110, 35);
            this.btnTransformToJSON.TabIndex = 8;
            this.btnTransformToJSON.Text = "XML => JSON";
            this.btnTransformToJSON.UseVisualStyleBackColor = true;
            this.btnTransformToJSON.Click += new System.EventHandler(this.btnTransformToJSON_Click);
            // 
            // labSelectedJSON
            // 
            this.labSelectedJSON.AutoSize = true;
            this.labSelectedJSON.Location = new System.Drawing.Point(226, 98);
            this.labSelectedJSON.Name = "labSelectedJSON";
            this.labSelectedJSON.Size = new System.Drawing.Size(112, 13);
            this.labSelectedJSON.TabIndex = 11;
            this.labSelectedJSON.Text = "No JSON was chosen";
            // 
            // btnSelectJSON
            // 
            this.btnSelectJSON.Location = new System.Drawing.Point(145, 93);
            this.btnSelectJSON.Name = "btnSelectJSON";
            this.btnSelectJSON.Size = new System.Drawing.Size(75, 23);
            this.btnSelectJSON.TabIndex = 10;
            this.btnSelectJSON.Text = "Choose...";
            this.btnSelectJSON.UseVisualStyleBackColor = true;
            this.btnSelectJSON.Click += new System.EventHandler(this.btnSelectJSON_Click);
            // 
            // labSelectJSON
            // 
            this.labSelectJSON.AutoSize = true;
            this.labSelectJSON.Location = new System.Drawing.Point(26, 98);
            this.labSelectJSON.Name = "labSelectJSON";
            this.labSelectJSON.Size = new System.Drawing.Size(84, 13);
            this.labSelectJSON.TabIndex = 9;
            this.labSelectJSON.Text = "Select JSON file";
            // 
            // btnJSONtoXML
            // 
            this.btnJSONtoXML.Enabled = false;
            this.btnJSONtoXML.Location = new System.Drawing.Point(261, 122);
            this.btnJSONtoXML.Name = "btnJSONtoXML";
            this.btnJSONtoXML.Size = new System.Drawing.Size(110, 35);
            this.btnJSONtoXML.TabIndex = 12;
            this.btnJSONtoXML.Text = "JSON => XML";
            this.btnJSONtoXML.UseVisualStyleBackColor = true;
            this.btnJSONtoXML.Click += new System.EventHandler(this.btnJSONtoXML_Click);
            // 
            // TestXSLT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 203);
            this.Controls.Add(this.btnJSONtoXML);
            this.Controls.Add(this.labSelectedJSON);
            this.Controls.Add(this.btnSelectJSON);
            this.Controls.Add(this.labSelectJSON);
            this.Controls.Add(this.btnTransformToJSON);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnTransform);
            this.Controls.Add(this.labSelectedXSLT);
            this.Controls.Add(this.labSelectedXML);
            this.Controls.Add(this.btnSelectXSLT);
            this.Controls.Add(this.btnSelectXML);
            this.Controls.Add(this.labSelectXSLT);
            this.Controls.Add(this.labSelectXml);
            this.Name = "TestXSLT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test XSLT";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labSelectXml;
        private System.Windows.Forms.Label labSelectXSLT;
        private System.Windows.Forms.Button btnSelectXML;
        private System.Windows.Forms.Button btnSelectXSLT;
        private System.Windows.Forms.Label labSelectedXML;
        private System.Windows.Forms.Label labSelectedXSLT;
        private System.Windows.Forms.Button btnTransform;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnTransformToJSON;
        private System.Windows.Forms.Label labSelectedJSON;
        private System.Windows.Forms.Button btnSelectJSON;
        private System.Windows.Forms.Label labSelectJSON;
        private System.Windows.Forms.Button btnJSONtoXML;
    }
}


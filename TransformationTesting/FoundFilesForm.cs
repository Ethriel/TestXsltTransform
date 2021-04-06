using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TransformationTesting.Utilities;

namespace TransformationTesting
{
    public partial class FoundFilesForm : Form
    {
        public TestXSLT OwnerForm;
        public FoundFilesForm(TestXSLT ownerForm)
        {
            InitializeComponent();
            OwnerForm = ownerForm;
            lbXml.DisplayMember = nameof(FileInfo.Name);
            lbXml.ValueMember = nameof(FileInfo.FullName);
            lbXslt.DisplayMember = nameof(FileInfo.Name);
            lbXslt.ValueMember = nameof(FileInfo.FullName);
        }

        public bool SearchFiles(string folder, params string[] patterns)
        {
            var search = new FileSearchEngine();
            var files = search.SearchFiles(folder, patterns);

            var xmlFiles = GetFileModels(files, ".xml");
            lbXml.DataSource = xmlFiles;

            var xsltFiles = GetFileModels(files, ".xslt");
            lbXslt.DataSource = xsltFiles;

            return xmlFiles.Any() && xsltFiles.Any();
        }

        private IList<FileInfo> GetFileModels(IEnumerable<FileInfo> files, string extension)
        {
            return files.Where(file => file.Extension == extension).ToList();
        }

        private void lbXml_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbXml.SelectedItem is FileInfo)
            {
                OwnerForm.Files.XmlFile = lbXml.SelectedItem as FileInfo;
            }
        }

        private void lbXslt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbXslt.SelectedItem is FileInfo)
            {
                OwnerForm.Files.XsltFile = lbXslt.SelectedItem as FileInfo;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TransformationTesting.Utilities;

namespace TransformationTesting
{
    public partial class TestXSLT : Form
    {
        private readonly string _rootFolder;
        private readonly string _noFileChosen;
        public FileInfo XmlFile { get; set; }
        public FileInfo XsltFile { get; set; }
        private readonly MessageBoxFactory _messageBoxFactory;
        public TestXSLT()
        {
            InitializeComponent();
            _rootFolder = Directory.GetCurrentDirectory();
            _noFileChosen = "No {0} file was chosen";
            _messageBoxFactory = new MessageBoxFactory();
            CreateFolders();
            EnableSearch("*.xml", "*.xslt");
        }

        private void btnSelectXML_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _rootFolder;
                ofd.Filter = "XML files|*.xml";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(ofd.FileName))
                    {
                        XmlFile = new FileInfo(ofd.FileName);
                        labSelectedXML.Text = ofd.FileName;
                        CheckTransformButton();
                    }
                }
            }
        }

        private void btnSelectXSLT_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _rootFolder;
                ofd.Filter = "XSLT files|*.xslt";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(ofd.FileName))
                    {
                        XsltFile = new FileInfo(ofd.FileName);
                        labSelectedXSLT.Text = ofd.FileName;
                        CheckTransformButton();
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnTransform_Click(object sender, EventArgs e)
        {
            try
            {
                SetTransformAndClearButtons(false);
                var transformXml = new TransformXmlWithXslt(XmlFile, XsltFile, _rootFolder);

                if (transformXml.Transform())
                {
                    if (_messageBoxFactory.ShowInfoBox("XML was transformed successfully", "Success") == DialogResult.OK)
                    {
                        if (_messageBoxFactory.ShowQuestionBox("Open transformed file?", "Info") == DialogResult.Yes)
                        {
                            var outFile = CombinePaths(_rootFolder, "out", XmlFile.Name);
                            Process.Start(outFile);
                        }
                    }
                    SetTransformAndClearButtons(true);
                }
                else
                {
                    if (_messageBoxFactory.ShowWarningBox("XML was not transformed", "Transformation error") == DialogResult.OK)
                    {
                        if (_messageBoxFactory.ShowQuestionBox("Show possible reasons?", "Show reasons?") == DialogResult.Yes)
                        {
                            var message = default(string);
                            foreach (var error in transformXml.Errors)
                            {
                                message += $"{error}\n";
                            }

                            _messageBoxFactory.ShowInfoBox(message, "Reasons");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (_messageBoxFactory.ShowErrorBox(ex.Message, "Error") == DialogResult.OK)
                {
                    if (_messageBoxFactory.ShowQuestionBox("Show stack trace?", "Info") == DialogResult.Yes)
                    {
                        _messageBoxFactory.ShowErrorBox(ex.StackTrace, "Stack trace");
                    }
                }
            }
            finally
            {
                Clear();
            }
        }

        private void CreateFolders()
        {
            CreateFolderInRootDirectory("in");
            CreateFolderInRootDirectory("out");
            CreateFolderInRootDirectory("xslt");
        }

        private void CreateFolderInRootDirectory(string folderName)
        {
            var path = CombinePaths(_rootFolder, folderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private string CombinePaths(params string[] paths)
        {
            return Path.Combine(paths);
        }

        private void SetLabsAndfiles()
        {
            if (XmlFile == null)
            {
                ClearXml();
            }
            else
            {
                labSelectedXML.Text = XmlFile.FullName;
            }

            if (XsltFile == null)
            {
                ClearXslt();
            }
            else
            {
                labSelectedXSLT.Text = XsltFile.FullName;
            }
            FileFoundBox();
        }

        private void FileFoundBox()
        {
            var message = "Files found: ";
            if (XmlFile != null)
            {
                message = string.Concat(message, XmlFile.Name, ", ");
                if (XsltFile != null)
                {
                    message = string.Concat(message, XsltFile.Name);
                }
            }
            else
            {
                message = "No files were found";
            }
            _messageBoxFactory.ShowInfoBox(message, "Files");
        }

        private void Clear()
        {
            ClearXml();
            ClearXslt();
            btnTransform.Enabled = false;
        }

        private void ClearXml()
        {
            XmlFile = null;
            labSelectedXML.Text = string.Format(_noFileChosen, "XML");
        }

        private void ClearXslt()
        {
            XsltFile = null;
            labSelectedXSLT.Text = string.Format(_noFileChosen, "XSLT");
        }

        private void SetTransformAndClearButtons(bool state)
        {
            btnTransform.Enabled = state;
            btnClear.Enabled = state;
        }

        private void EnableSearch(params string[] patterns)
        {
            var message = $"Would you like the program to search for {GetPatternsAsString(patterns)} files itself?";
            if (_messageBoxFactory.ShowQuestionBox(message, "Search?") == DialogResult.Yes)
            {
                var foundForm = new FoundFilesForm(this);
                var found = foundForm.SearchFiles(_rootFolder, patterns);
                if (found)
                {
                    foundForm.ShowDialog();
                }
                SetLabsAndfiles();
                CheckTransformButton();
            }
        }

        private string GetPatternsAsString(params string[] patterns)
        {
            return string.Join(" ", patterns);
        }

        private void CheckTransformButton()
        {
            btnTransform.Enabled = CheckIfReady();
        }

        private bool CheckIfReady()
        {
            return IsFileInfoReady(XmlFile) && IsFileInfoReady(XsltFile);
        }

        private bool IsFileInfoReady(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                return false;
            }

            return !IsStringNullOrEmpty(fileInfo.FullName);
        }

        private bool IsStringNullOrEmpty(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}

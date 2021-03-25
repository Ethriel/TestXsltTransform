using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Xsl;
using TransformationTesting.Utilities;

namespace TransformationTesting
{
    public partial class TestXSLT : Form
    {
        private readonly string _rootFolder;
        private string _noFileChosen;
        private FileInfo _xmlFile;
        private FileInfo _xsltFile;
        private FileSearchEngine _fileSearch;
        private readonly MessageBoxFactory _messageMoxFactory;
        public TestXSLT()
        {
            InitializeComponent();
            _rootFolder = Directory.GetCurrentDirectory();
            _noFileChosen = "No {0} file was chosen";
            _messageMoxFactory = new MessageBoxFactory();
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
                        _xmlFile = new FileInfo(ofd.FileName);
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
                        _xsltFile = new FileInfo(ofd.FileName);
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
                var transformXml = new TransformXmlWithXslt(_xmlFile, _xsltFile, _rootFolder);

                if (transformXml.Transform())
                {
                    if (_messageMoxFactory.ShowInfoBox("XML was transformed successfully", "Success") == DialogResult.OK)
                    {
                        if (_messageMoxFactory.ShowQuestionBox("Open transformed file?", "Info") == DialogResult.Yes)
                        {
                            var outFile = CombinePaths(_rootFolder, "out", _xmlFile.Name);
                            Process.Start(outFile);
                        }
                    }
                    SetTransformAndClearButtons(true);
                }
                else
                {
                    if (_messageMoxFactory.ShowWarningBox("XML was not transformed", "Transformation error") == DialogResult.OK)
                    {
                        if (_messageMoxFactory.ShowQuestionBox("Show possible reasons?", "Show reasons?") == DialogResult.Yes)
                        {
                            var message = default(string);
                            foreach (var error in transformXml.Errors)
                            {
                                message += $"{error}\n";
                            }

                            _messageMoxFactory.ShowInfoBox(message, "Reasons");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (_messageMoxFactory.ShowErrorBox(ex.Message, "Error") == DialogResult.OK)
                {
                    if (_messageMoxFactory.ShowQuestionBox("Show stack trace?", "Info") == DialogResult.Yes)
                    {
                        _messageMoxFactory.ShowErrorBox(ex.StackTrace, "Stack trace");
                    }
                }
            }
            finally
            {
                Clear();
            }
        }

        private bool CheckIfReady()
        {
            return IsFileInfoReady(_xmlFile) && IsFileInfoReady(_xsltFile);
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
            if (_xmlFile == null)
            {
                ClearXml();
            }
            else
            {
                labSelectedXML.Text = _xmlFile.FullName;
            }

            if (_xsltFile == null)
            {
                ClearXslt();
            }
            else
            {
                labSelectedXSLT.Text = _xsltFile.FullName;
            }
            FileFoundBox();
        }

        private void FileFoundBox()
        {
            var message = "Files found: ";
            if (_xmlFile != null)
            {
                message = string.Concat(message, _xmlFile.Name, ", ");
                if (_xsltFile != null)
                {
                    message = string.Concat(message, _xsltFile.Name);
                }
            }
            else
            {
                message = "No files were found";
            }
            _messageMoxFactory.ShowInfoBox(message, "Files");
        }

        private void Clear()
        {
            ClearXml();
            ClearXslt();
            btnTransform.Enabled = false;
        }

        private void ClearXml()
        {
            _xmlFile = null;
            labSelectedXML.Text = string.Format(_noFileChosen, "XML");
        }

        private void ClearXslt()
        {
            _xsltFile = null;
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
            if (_messageMoxFactory.ShowQuestionBox(message, "Search?") == DialogResult.Yes)
            {
                _fileSearch = new FileSearchEngine();
                SearchForFiles(patterns);
                SetLabsAndfiles();
                CheckTransformButton();
            }
        }

        private string GetPatternsAsString(params string[] patterns)
        {
            return string.Join(" ", patterns);
        }

        private void SearchForFiles(params string[] patterns)
        {
            var files = _fileSearch.SearchFiles(_rootFolder, patterns);

            _xmlFile = files.FirstOrDefault(file => file.Extension == ".xml");
            _xsltFile = files.FirstOrDefault(file => file.Extension == ".xslt");
        }

        private void CheckTransformButton()
        {
            btnTransform.Enabled = CheckIfReady();
        }
    }
}

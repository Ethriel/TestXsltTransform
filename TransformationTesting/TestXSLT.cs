using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TransformationTesting.Utilities;

namespace TransformationTesting
{
    public partial class TestXSLT : Form
    {
        private readonly string rootFolder;
        private readonly string noFileChosen;
        private readonly TransformEngine transformEngine;
        private readonly MessageBoxFactory messageBoxFactory;
        public FileInfo XmlFile { get; set; }
        public FileInfo XsltFile { get; set; }
        public FileInfo JsonFile { get; set; }
        public TestXSLT()
        {
            InitializeComponent();
            rootFolder = Directory.GetCurrentDirectory();
            noFileChosen = "No {0} file was chosen";
            messageBoxFactory = new MessageBoxFactory();
            transformEngine = new TransformEngine();
            CreateFolders();
            EnableSearch("*.xml", "*.xslt");
        }

        private void btnSelectXML_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = rootFolder;
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
                ofd.InitialDirectory = rootFolder;
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

        private void btnSelectJSON_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = rootFolder;
                ofd.Filter = "JSON files|*.json";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(ofd.FileName))
                    {
                        JsonFile = new FileInfo(ofd.FileName);
                        labSelectedJSON.Text = ofd.FileName;
                        btnJSONtoXML.Enabled = CheckIfJsonReady();
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
                if (transformEngine.TransformXmlWithXslt(XmlFile, XsltFile.FullName, rootFolder))
                {
                    if (messageBoxFactory.ShowInfoBox("XML was transformed successfully", "Success") == DialogResult.OK)
                    {
                        if (messageBoxFactory.ShowQuestionBox("Open transformed file?", "Info") == DialogResult.Yes)
                        {
                            var outFile = CombinePaths(rootFolder, "out", XmlFile.Name);
                            Process.Start(outFile);
                        }
                    }
                    SetTransformAndClearButtons(true);
                }
                else
                {
                    if (messageBoxFactory.ShowWarningBox("XML was not transformed", "Transformation error") == DialogResult.OK)
                    {
                        if (messageBoxFactory.ShowQuestionBox("Show possible reasons?", "Show reasons?") == DialogResult.Yes)
                        {
                            ShowReasons();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (messageBoxFactory.ShowErrorBox(ex.Message, "Error") == DialogResult.OK)
                {
                    if (messageBoxFactory.ShowQuestionBox("Show stack trace?", "Info") == DialogResult.Yes)
                    {
                        messageBoxFactory.ShowErrorBox(ex.StackTrace, "Stack trace");
                    }
                }
            }
            finally
            {
                Clear();
            }
        }

        private void btnTransformToJSON_Click(object sender, EventArgs e)
        {
            try
            {
                if (transformEngine.TransformXmlToJson(XmlFile.FullName, rootFolder))
                {
                    if (messageBoxFactory.ShowInfoBox("XML was transformed successfully", "Success") == DialogResult.OK)
                    {
                        if (messageBoxFactory.ShowQuestionBox("Open transformed file?", "Info") == DialogResult.Yes)
                        {
                            var fileName = string.Concat(Path.GetFileNameWithoutExtension(XmlFile.FullName), ".json");
                            var outFile = CombinePaths(rootFolder, "out", "json", fileName);
                            Process.Start(outFile);
                        }
                    }
                }
                else
                {
                    if (messageBoxFactory.ShowWarningBox("XML was not transformed", "Transformation error") == DialogResult.OK)
                    {
                        if (messageBoxFactory.ShowQuestionBox("Show possible reasons?", "Show reasons?") == DialogResult.Yes)
                        {
                            ShowReasons();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (messageBoxFactory.ShowErrorBox(ex.Message, "XML transformation error") == DialogResult.OK)
                {
                    if (messageBoxFactory.ShowQuestionBox("Show stack trace?", "Info") == DialogResult.Yes)
                    {
                        messageBoxFactory.ShowErrorBox(ex.StackTrace, "Stack trace");
                    }
                }
            }
            finally
            {
                Clear();
            }
        }
        
        private void btnJSONtoXML_Click(object sender, EventArgs e)
        {
            try
            {
                if (transformEngine.TransformJsonToXml(JsonFile.FullName, rootFolder))
                {
                    if (messageBoxFactory.ShowInfoBox("JSON was transformed successfully", "Success") == DialogResult.OK)
                    {
                        if (messageBoxFactory.ShowQuestionBox("Open transformed file?", "Info") == DialogResult.Yes)
                        {
                            var fileName = string.Concat(Path.GetFileNameWithoutExtension(JsonFile.FullName), ".xml");
                            var outFile = CombinePaths(rootFolder, "out", fileName);
                            Process.Start(outFile);
                        }
                    }
                }
                else
                {
                    if (messageBoxFactory.ShowWarningBox("XML was not transformed", "Transformation error") == DialogResult.OK)
                    {
                        if (messageBoxFactory.ShowQuestionBox("Show possible reasons?", "Show reasons?") == DialogResult.Yes)
                        {
                            ShowReasons();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (messageBoxFactory.ShowErrorBox(ex.Message, "XML transformation error") == DialogResult.OK)
                {
                    if (messageBoxFactory.ShowQuestionBox("Show stack trace?", "Info") == DialogResult.Yes)
                    {
                        messageBoxFactory.ShowErrorBox(ex.StackTrace, "Stack trace");
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
            CreateFolderInRootDirectory("json");
        }

        private void ShowReasons()
        {
            messageBoxFactory.ShowInfoBox(transformEngine.GetErrorsAsString(), "Reasons");
        }

        private void CreateFolderInRootDirectory(string folderName)
        {
            var path = CombinePaths(rootFolder, folderName);
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
            messageBoxFactory.ShowInfoBox(message, "Files");
        }

        private void Clear()
        {
            ClearXml();
            ClearXslt();
            ClearJson();
            btnTransform.Enabled = false;
        }

        private void ClearXml()
        {
            XmlFile = null;
            labSelectedXML.Text = string.Format(noFileChosen, "XML");
        }

        private void ClearXslt()
        {
            XsltFile = null;
            labSelectedXSLT.Text = string.Format(noFileChosen, "XSLT");
        }

        private void ClearJson()
        {
            JsonFile = null;
            labSelectedJSON.Text = string.Format(noFileChosen, "JSON");
            btnJSONtoXML.Enabled = false;
        }

        private void SetTransformAndClearButtons(bool state)
        {
            btnTransform.Enabled = state;
            btnClear.Enabled = state;
        }

        private void EnableSearch(params string[] patterns)
        {
            var message = $"Would you like the program to search for {GetPatternsAsString(patterns)} files itself?";
            if (messageBoxFactory.ShowQuestionBox(message, "Search?") == DialogResult.Yes)
            {
                var foundForm = new FoundFilesForm(this);
                var found = foundForm.SearchFiles(rootFolder, patterns);
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

        private bool CheckIfJsonReady()
        {
            return IsFileInfoReady(JsonFile);
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

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
        private FileInfo xmlFile;
        private FileInfo xsltFile;
        private FileInfo jsonFile;
        public FileInfo XmlFile { get => xmlFile; set => xmlFile = value; }
        public FileInfo XsltFile { get => xsltFile; set => xsltFile = value; }
        public FileInfo JsonFile { get => jsonFile; set => jsonFile = value; }
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
            SelectFile("XML files|*.xml", ref xmlFile, ref labSelectedXML);
        }

        private void btnSelectXSLT_Click(object sender, EventArgs e)
        {
            SelectFile("XSLT files|*.xslt", ref xsltFile, ref labSelectedXSLT);
        }

        private void btnSelectJSON_Click(object sender, EventArgs e)
        {
            SelectFile("JSON files|*.json", ref jsonFile, ref labSelectedJSON);
        }

        private void SelectFile(string filter, ref FileInfo file, ref Label label)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = rootFolder;
                ofd.Filter = filter;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(ofd.FileName))
                    {
                        file = new FileInfo(ofd.FileName);
                        label.Text = ofd.FileName;
                        CheckTransformButtons();
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
                Transform(TransformType.XmlWithXslt);
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
            finally
            {
                Clear();
                CheckTransformButtons();
            }
        }

        private void btnXmlToJSON_Click(object sender, EventArgs e)
        {
            try
            {
                Transform(TransformType.XmlToJson);
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
            finally
            {
                Clear();
                CheckTransformButtons();
            }
        }

        private void btnJSONtoXML_Click(object sender, EventArgs e)
        {
            try
            {
                Transform(TransformType.JsonToXml);
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
            finally
            {
                Clear();
                CheckTransformButtons();
            }
        }

        private void Transform(TransformType transformType)
        {
            var transformResult = false;
            var fileWitoutExtension = default(string);
            var fileName = default(string);

            switch (transformType)
            {

                case TransformType.XmlWithXslt:
                    transformResult = transformEngine.TransformXmlWithXslt(XmlFile, XsltFile.FullName, rootFolder);
                    fileName = CombinePaths(rootFolder, "out", XmlFile.Name);
                    break;
                case TransformType.JsonToXml:
                    transformResult = transformEngine.TransformJsonToXml(JsonFile.FullName, rootFolder);
                    fileWitoutExtension = Path.GetFileNameWithoutExtension(JsonFile.FullName);
                    fileName = CombinePaths(rootFolder, "out", string.Concat(fileWitoutExtension, ".xml"));
                    break;
                case TransformType.XmlToJson:
                    transformResult = transformEngine.TransformXmlToJson(XmlFile.FullName, rootFolder);
                    fileWitoutExtension = Path.GetFileNameWithoutExtension(XmlFile.FullName);
                    fileName = CombinePaths(rootFolder, "out", string.Concat(fileWitoutExtension, ".json"));
                    break;
                default:
                    break;
            }

            if (transformResult)
            {
                SetTransformAndClearButtons(false);
                if (messageBoxFactory.ShowInfoBox("File was transformed successfully", "Success") == DialogResult.OK)
                {
                    if (messageBoxFactory.ShowQuestionBox("Open transformed file?", "Info") == DialogResult.Yes)
                    {
                        Process.Start(fileName);
                    }
                }
                SetTransformAndClearButtons(true);
            }
            else
            {
                if (messageBoxFactory.ShowWarningBox("File was not transformed", "Transformation error") == DialogResult.OK)
                {
                    if (messageBoxFactory.ShowQuestionBox("Show possible reasons?", "Show reasons?") == DialogResult.Yes)
                    {
                        ShowReasons();
                    }
                }
            }
        }

        private void ShowException(Exception ex)
        {
            if (messageBoxFactory.ShowErrorBox(ex.Message, "Error") == DialogResult.OK)
            {
                if (messageBoxFactory.ShowQuestionBox("Show stack trace?", "Info") == DialogResult.Yes)
                {
                    messageBoxFactory.ShowErrorBox(ex.StackTrace, "Stack trace");
                }
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
            if (messageBoxFactory.ShowWarningBox("XML was not transformed", "Transformation error") == DialogResult.OK)
            {
                if (messageBoxFactory.ShowQuestionBox("Show possible reasons?", "Show reasons?") == DialogResult.Yes)
                {
                    messageBoxFactory.ShowInfoBox(transformEngine.GetErrorsAsString(), "Reasons");
                }
            }
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
        }

        private void SetTransformAndClearButtons(bool state)
        {
            btnTransform.Enabled = state;
            btnClear.Enabled = state;
            btnJSONtoXML.Enabled = state;
            btnXmlToJSON.Enabled = state;
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
                CheckTransformButtons();
            }
        }

        private string GetPatternsAsString(params string[] patterns)
        {
            return string.Join(" ", patterns);
        }

        private void CheckTransformButtons()
        {
            btnTransform.Enabled = CheckIfReady();
            btnJSONtoXML.Enabled = CheckIfJsonReady();
            btnXmlToJSON.Enabled = IsFileInfoReady(XmlFile);
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

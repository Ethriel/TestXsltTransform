using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TransformationTesting.Utilities;

namespace TransformationTesting
{
    public partial class TestXSLT : Form
    {
        private readonly TransformEngine transformEngine;
        private readonly MessageBoxFactory messageBoxFactory;
        private readonly FilesToTransform files;
        public FilesToTransform Files { get => files; }
        public TestXSLT()
        {
            InitializeComponent();
            messageBoxFactory = new MessageBoxFactory();
            transformEngine = new TransformEngine();
            files = new FilesToTransform();
            CreateFolders();
            EnableSearch("*.xml", "*.xslt");
        }

        private void btnSelectXML_Click(object sender, EventArgs e)
        {
            files.XmlFile = SelectFile("XML files|*.xml", labSelectedXML);
            CheckTransformButtons();
        }

        private void btnSelectXSLT_Click(object sender, EventArgs e)
        {
            files.XsltFile = SelectFile("XSLT files|*.xslt", labSelectedXSLT);
            CheckTransformButtons();
        }

        private void btnSelectJSON_Click(object sender, EventArgs e)
        {
            files.JsonFile = SelectFile("JSON files|*.json", labSelectedJSON);
            CheckTransformButtons();
        }

        private FileInfo SelectFile(string filter, Label label)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = files.RootDirectory;
                ofd.Filter = filter;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(ofd.FileName))
                    {
                        label.Text = ofd.FileName;
                        return new FileInfo(ofd.FileName);
                    }
                }

                return null;
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
            var fileName = default(string);

            switch (transformType)
            {

                case TransformType.XmlWithXslt:
                    transformResult = transformEngine.TransformXmlWithXslt(files.XmlFile, files.XsltFile.FullName, files.RootDirectory);
                    fileName = files.FileNameOutXml;
                    break;
                case TransformType.JsonToXml:
                    transformResult = transformEngine.TransformJsonToXml(files.JsonFile.FullName, files.RootDirectory);
                    fileName = files.FileNameOutXmlFromJson;
                    break;
                case TransformType.XmlToJson:
                    transformResult = transformEngine.TransformXmlToJson(files.XmlFile.FullName, files.RootDirectory);
                    fileName = files.FileNameOutJson;
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
            var path = Path.Combine(files.RootDirectory, folderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void SetLabsAndfiles()
        {
            if (files.XmlFile == null)
            {
                ClearXml();
            }
            else
            {
                labSelectedXML.Text = files.XmlFile.FullName;
            }

            if (files.XsltFile == null)
            {
                ClearXslt();
            }
            else
            {
                labSelectedXSLT.Text = files.XsltFile.FullName;
            }

            if (files.JsonFile == null)
            {
                ClearJson();
            }
            else
            {
                labSelectJSON.Text = files.JsonFile.FullName;
            }
            FileFoundBox();
        }

        private void FileFoundBox()
        {
            var message = "Files found: ";
            if (files.XmlFile != null)
            {
                message = string.Concat(message, files.XmlFile.Name, ", ");
                if (files.XsltFile != null)
                {
                    message = string.Concat(message, files.XsltFile.Name);
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
            btnXmlWithXslt.Enabled = false;
        }

        private void ClearXml()
        {
            labSelectedXML.Text = files.ClearXml();
        }

        private void ClearXslt()
        {
            labSelectedXSLT.Text = files.ClearXslt();
        }

        private void ClearJson()
        {
            labSelectedJSON.Text = files.ClearJson();
        }

        private void SetTransformAndClearButtons(bool state)
        {
            btnXmlWithXslt.Enabled = state;
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
                var found = foundForm.SearchFiles(files.RootDirectory, patterns);
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
            btnXmlWithXslt.Enabled = files.IsXmlReadyForXslt();
            btnJSONtoXML.Enabled = files.IsJsonReadyForXml();
            btnXmlToJSON.Enabled = files.IsXmlReadyForJson();
        }
    }
}

using System.IO;

namespace TransformationTesting.Utilities
{
    public class FilesToTransform
    {
        private readonly string rootDirectory;
        private readonly string noFileChosen;
        private readonly string outDirectory;
        private FileInfo xmlFile;
        private FileInfo xsltFile;
        private FileInfo jsonFile;
        public FileInfo XmlFile { get => xmlFile; set => xmlFile = value; }
        public FileInfo XsltFile { get => xsltFile; set => xsltFile = value; }
        public FileInfo JsonFile { get => jsonFile; set => jsonFile = value; }
        public string XmlWithoutExtension { get => FileNameWithOutExtension(XmlFile); }
        public string XsltWithoutExtension { get => FileNameWithOutExtension(XsltFile); }
        public string JsonWithoutExtension { get => FileNameWithOutExtension(JsonFile); }
        public string FileNameOutJson { get => CombinePaths(outDirectory, ConcatStrings(XmlWithoutExtension, ".json")); }
        public string FileNameOutXml { get => CombinePaths(outDirectory, XmlFile.Name); }
        public string FileNameOutXmlFromJson { get => CombinePaths(outDirectory, ConcatStrings(JsonWithoutExtension, ".xml")); }
        public string RootDirectory { get => rootDirectory; }
        public string OutDirectory { get => outDirectory; }
        public FilesToTransform()
        {
            rootDirectory = Directory.GetCurrentDirectory();
            outDirectory = CombinePaths(rootDirectory, "out");
            noFileChosen = "No {0} file was chosen";
        }

        public string NoFileChosen(string file)
        {
            return string.Format(noFileChosen, file);
        }

        public string ClearXml()
        {
            XmlFile = null;
            return NoFileChosen("XML");
        }

        public string ClearXslt()
        {
            XsltFile = null;
            return NoFileChosen("XSLT");
        }

        public string ClearJson()
        {
            JsonFile = null;
            return NoFileChosen("JSON");
        }

        public bool IsXmlReadyForXslt()
        {
            return IsFileInfoReady(XmlFile) && IsFileInfoReady(XsltFile);
        }

        public bool IsXmlReadyForJson()
        {
            return IsFileInfoReady(XmlFile);
        }

        public bool IsJsonReadyForXml()
        {
            return IsFileInfoReady(JsonFile);
        }

        private string FileNameWithOutExtension(FileInfo file)
        {
            return Path.GetFileNameWithoutExtension(file.FullName);
        }

        private string ConcatStrings(params string[] strings)
        {
            return string.Concat(strings);
        }

        private string CombinePaths(params string[] paths)
        {
            return Path.Combine(paths);
        }

        private bool IsFileInfoReady(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(fileInfo.FullName);
        }
    }
}

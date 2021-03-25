using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;

namespace TransformationTesting.Utilities
{
    public class TransformXmlWithXslt
    {
        private readonly FileInfo xmlFile;
        private readonly FileInfo xsltFile;
        private readonly string rootFolder;
        public IEnumerable<string> Errors { get; set; }

        public TransformXmlWithXslt()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// Create an instance of TransformXmlWithXslt class with <paramref name="xmlFile"/>, <paramref name="xsltFile"/> and <paramref name="rootFolder"/>
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="xsltFile"></param>
        /// <param name="rootFolder"></param>
        public TransformXmlWithXslt(FileInfo xmlFile, FileInfo xsltFile, string rootFolder)
        {
            this.xmlFile = xmlFile;
            this.xsltFile = xsltFile;
            this.rootFolder = rootFolder;
            Errors = new List<string>();
        }

        /// <summary>
        /// Transform XML with XSLT
        /// </summary>
        /// <returns></returns>
        public bool Transform()
        {
            try
            {
                var transformed = false;
                var dataToTransform = GetDataToTransform();

                if (dataToTransform.Length > 0)
                {
                    var regexPattern = "{newguid}";
                    var regex = new Regex(regexPattern);
                    while (dataToTransform.IndexOf(regexPattern, 0) > -1)
                    {
                        dataToTransform = regex.Replace(dataToTransform, Guid.NewGuid().ToString(), 1);
                    }

                    var transformedData = TransformString(dataToTransform);

                    if (transformedData.Length > 0)
                    {
                        var document = new XmlDocument();
                        document.LoadXml(transformedData);

                        var outFile = Path.Combine(rootFolder, "out", xmlFile.Name);
                        using (var writer = new XmlTextWriter(outFile, null))
                        {
                            writer.Formatting = Formatting.Indented;
                            document.Save(writer);
                        }
                        transformed = true;
                    }
                    else
                    {
                        AddError("Transformation output is empty!");
                    }
                }
                else
                {
                    AddError("There is nothing to transform!");
                }

                return transformed;
            }
            catch (XsltException xsltEx)
            {
                AddError(xsltEx.Message);
                if (xsltEx.InnerException != null)
                {
                    AddError(xsltEx.InnerException.Message);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AddError(string error)
        {
            (Errors as List<string>).Add(error);
        }

        private XslCompiledTransform LoadCompiledTransform()
        {
            var xslCompiledTransform = new XslCompiledTransform();
            var xsltSettings = new XsltSettings(true, true);

            xslCompiledTransform.Load(xsltFile.FullName, xsltSettings, new XmlUrlResolver());

            return xslCompiledTransform;
        }

        private string GetDataToTransform()
        {
            var dataToTransform = string.Empty;
            using (var fileStream = File.OpenRead(xmlFile.FullName))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    dataToTransform = reader.ReadToEnd();
                }
            }

            return dataToTransform;
        }

        private string TransformString(string data)
        {
            var xslTransform = LoadCompiledTransform();
            var pXsltArgumentList = new XsltArgumentList();
            var stringBuilder = new StringBuilder();
            pXsltArgumentList.Clear();

            var xmlBytes = Encoding.ASCII.GetBytes(data);
            using (var memoryStream = new MemoryStream(xmlBytes))
            {
                var xSettings = new XmlReaderSettings();
                xSettings.ConformanceLevel = ConformanceLevel.Fragment;
                using (var xmlReader = XmlReader.Create(memoryStream, xSettings))
                {
                    XmlWriterSettings xmlWriterSettings = null;
                    using (var xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
                    {
                        xslTransform.Transform(xmlReader, pXsltArgumentList, xmlWriter);
                    }
                }
            }

            return stringBuilder.ToString();
        }
    }
}

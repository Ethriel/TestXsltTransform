using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;

namespace TransformationTesting.Utilities
{
    public enum TransformType
    {
        XmlWithXslt,
        JsonToXml,
        XmlToJson
    }
    public class TransformEngine
    {
        public ICollection<string> Errors { get; set; }

        public TransformEngine()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// Transform XML with XSLT
        /// </summary>
        /// <returns></returns>
        public bool TransformXmlWithXslt(FileInfo xmlFile, string xsltFile, string rootFolder)
        {
            try
            {
                ClearErrors();
                var transformed = false;
                var dataToTransform = GetDataToTransform(xmlFile);

                if (dataToTransform.Length > 0)
                {
                    var regexPattern = "{newguid}";
                    var regex = new Regex(regexPattern);
                    while (dataToTransform.IndexOf(regexPattern, 0) > -1)
                    {
                        dataToTransform = regex.Replace(dataToTransform, Guid.NewGuid().ToString(), 1);
                    }

                    var transformedData = TransformString(dataToTransform, xsltFile);

                    if (transformedData.Length > 0)
                    {
                        var document = new XmlDocument();
                        document.LoadXml(transformedData);

                        var outFile = Path.Combine(rootFolder, "out", xmlFile.Name);
                        using (var writer = new XmlTextWriter(outFile, null))
                        {
                            writer.Formatting = System.Xml.Formatting.Indented;
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
                AddError(ex.Message);
                throw;
            }
        }

        public bool TransformXmlToJson(string xmlFile, string rootDirectory)
        {
            try
            {
                ClearErrors();
                var xmlText = File.ReadAllText(xmlFile);
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlText);
                var json = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);
                var fileName = string.Concat(Path.GetFileNameWithoutExtension(xmlFile), ".json");
                var path = Path.Combine(rootDirectory, "out", fileName);
                File.WriteAllText(path, json);
                return true;
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                return false;
            }
        }

        public bool TransformJsonToXml(string jsonFile, string rootDirectory)
        {
            try
            {
                ClearErrors();
                var json = File.ReadAllText(jsonFile);
                var xmlDoc = JsonConvert.DeserializeXmlNode(json);
                var fileName = string.Concat(Path.GetFileNameWithoutExtension(jsonFile), ".xml");
                var path = Path.Combine(rootDirectory, "out", fileName);

                using (var writer = new XmlTextWriter(path, null))
                {
                    writer.Formatting = System.Xml.Formatting.Indented;
                    xmlDoc.Save(writer);
                }

                return true;
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                return false;
            }
        }

        public string GetErrorsAsString()
        {
            var message = default(string);
            foreach (var error in Errors)
            {
                message += $"{error}\n";
            }

            return message;
        }

        private void AddError(string error)
        {
            Errors.Add(error);
        }

        private void ClearErrors()
        {
            Errors.Clear();
        }

        private XslCompiledTransform LoadCompiledTransform(string xsltFile)
        {
            var xslCompiledTransform = new XslCompiledTransform();
            var xsltSettings = new XsltSettings(true, true);

            xslCompiledTransform.Load(xsltFile, xsltSettings, new XmlUrlResolver());

            return xslCompiledTransform;
        }

        private string GetDataToTransform(FileInfo xmlFile)
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

        private string TransformString(string data, string xsltFile)
        {
            var xslTransform = LoadCompiledTransform(xsltFile);
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

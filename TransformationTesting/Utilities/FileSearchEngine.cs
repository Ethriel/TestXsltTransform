using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TransformationTesting.Utilities
{
    public class FileSearchEngine
    {
        /// <summary>
        /// Search for files in <paramref name="folder"/> with specified <paramref name="pattern"/>
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public IEnumerable<FileInfo> SearchFiles(string pattern, string folder)
        {
            if (!Directory.Exists(folder))
            {
                throw new DirectoryNotFoundException($"{folder} does not exist!");
            }
            var fileNames = Directory.GetFiles(folder, pattern, SearchOption.AllDirectories);

            return fileNames.Select(name => new FileInfo(name));
        }

        /// <summary>
        /// Search for files in <paramref name="folder"/> with specified <paramref name="patterns"/>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="patterns"></param>
        /// <returns></returns>
        public IEnumerable<FileInfo> SearchFiles(string folder, params string[] patterns)
        {
            var files = new List<FileInfo>();
            var foundFiles = default(IEnumerable<FileInfo>);

            foreach (var pattern in patterns)
            {
                foundFiles = SearchFiles(pattern, folder);
                files.AddRange(foundFiles);
            }

            return files;
        }

        /// <summary>
        /// Search for file in <paramref name="folder"/> with specified <paramref name="pattern"/>
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public FileInfo SearchFirstFile(string pattern, string folder)
        {
            return SearchFiles(pattern, folder).FirstOrDefault();
        }
    }
}

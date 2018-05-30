using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WordsFinder.Logic
{
    public class FileFinder
    {
        public static List<FileInfo> FindFiles(string path)
        {
            List<FileInfo> result = new List<FileInfo>();
            var dir = new DirectoryInfo(path);
            foreach (FileInfo file in dir.GetFiles())
            {
                result.Add(file);
            }

            return result;
        }
    }
}

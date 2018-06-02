using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WordsFinder.Logic
{
    public class FileFinder
    {
        private static Stack<FileInfo> fileInfos = new Stack<FileInfo>();

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

        public static Stack<FileInfo> DirSearch(string sDir,string extension)
        {
            try
            {
                var path = new DirectoryInfo(sDir);

                var curentFiles = path.GetFiles($"*.{extension}");
                foreach (var file in curentFiles)
                {
                     fileInfos.Push(file);
                }

                var directories = path.GetDirectories();
                foreach (var d in directories)
                {
                    DirSearch(d.FullName,extension);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

            return fileInfos;
        }
    }
}

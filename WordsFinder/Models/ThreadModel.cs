using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace WordsFinder
{
    public class ThreadModel
    {
        public FileInfo fileInfo { get; }
        public static Dictionary<string, int> generalDictionary = new Dictionary<string, int>();

        public ThreadModel(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

    }
}

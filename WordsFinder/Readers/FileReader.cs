using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WordsFinder.Readers
{
    public class FileReader : IFileReader
    {
        public string ReadFromFile(string path)
        {
            String result = File.ReadAllText(path);

            return result;
        }
    }
}

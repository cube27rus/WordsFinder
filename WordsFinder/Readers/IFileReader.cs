using System;
using System.Collections.Generic;
using System.Text;

namespace WordsFinder.Readers
{
    public interface IFileReader
    {
        string ReadFromFile(string path);
    }
}

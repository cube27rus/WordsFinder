using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Threading;
using WordsFinder.Logic;


namespace WordsFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<FileInfo> fileInfos = new Stack<FileInfo>();
            foreach (var file in FileFinder.FindFiles("..\\..\\..\\files"))
            {
                fileInfos.Push(file);
            }
            ThreadMaker threadMaker = new ThreadMaker(fileInfos, 10);
            threadMaker.MakeThreads();

            Thread.Sleep(1000);
            Console.WriteLine("Нажмите любую клавишу, чтобы увидеть результаты");
            
            
            Console.ReadKey();

            ThreadModel.generalDictionary = ThreadModel.generalDictionary.SaveTenLargestValues();

            foreach (KeyValuePair<string, int> entry in ThreadModel.generalDictionary)
            {
                Console.WriteLine("word - {0}, count - {1}", entry.Key, entry.Value);
            }

            Console.ReadKey();

        }
    }
}

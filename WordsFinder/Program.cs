using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Threading;
using WordsFinder.Logic;
using WordsFinder.Readers;

namespace WordsFinder
{
    class Program
    {
        private static FileReader fileReader = new FileReader();
        private static Dictionary<string, int> generalDictionary = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            foreach (var file in FileFinder.FindFiles("C:\\Users\\Bond\\source\\repos\\WordsFinder\\WordsFinder\\files"))
            {
                Thread thread = new Thread(DoWork);
                thread.Start(file);
            }

            Console.ReadKey();

            Program.generalDictionary = DictionaryUpdater.SaveTenLargestValues(Program.generalDictionary);

            foreach (KeyValuePair<string, int> entry in Program.generalDictionary)
            {
                Console.WriteLine("key - {0}, value - {1}",entry.Key,entry.Value);
            }

            Console.ReadKey();

        }

        public static void DoWork(Object file)
        {
            FileInfo fileInfo = file as FileInfo;
            String text = fileReader.ReadFromFile(fileInfo.FullName);

            Dictionary<string, int> longestWords = WordFinder.FindLongestWord(text, 10);

            lock (generalDictionary)
            {
                DictionaryUpdater.MergeTwoDictionary(ref generalDictionary, longestWords);
            }
        }

        public static void DoWork(FileReader fileReader,FileInfo file, ref Dictionary<string, int> generalDictionary)
        {
            String text = fileReader.ReadFromFile(file.FullName);

            Dictionary<string, int> longestWords = WordFinder.FindLongestWord(text, 10);

            DictionaryUpdater.MergeTwoDictionary(ref generalDictionary, longestWords);
        }
       
    }
}

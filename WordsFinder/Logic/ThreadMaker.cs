using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WordsFinder.Logic
{
    public class ThreadMaker
    {
        public static Stack<FileInfo> fileInfos;
        private int countOfThreads;
        public static object locker = new object();

        public ThreadMaker(int countOfThreads)
        {
            this.countOfThreads = countOfThreads;
        }

        public void MakeThreads()
        {
            for (int i = 0; i < countOfThreads; i++)
            {
                if (fileInfos.Count != 0)
                {
                    Thread thread = new Thread(RunWorkThread);
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
        }

        public void MakeThreadpoolThreads(int completionPortThreads)
        {
            ThreadPool.SetMaxThreads(countOfThreads, completionPortThreads);
            while (fileInfos.Count != 0)
            {
                ThreadPool.QueueUserWorkItem(RunWork, fileInfos.Pop());
            }
        }

        public void MakeParallelLoopThreads()
        {
            ParallelLoopResult result = Parallel.ForEach<FileInfo>(fileInfos, new ParallelOptions { MaxDegreeOfParallelism = countOfThreads },DoWork);
        }
        
        public static void DoWork(FileInfo fileInfo)
        {
            Dictionary<string, int> longestWords = new Dictionary<string, int>();
            ThreadModel threadModel = new ThreadModel(fileInfo);
            using (FileStream fs = File.Open(threadModel.fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            lock (locker)
                            {
                                DictionaryUpdater.AppendDictionary(longestWords, WordFinder.FindLongestWord(line, 10));
                            }
                            
                        }
                    }
                }
            }

            DictionaryUpdater.MergeTwoDictionary(ref ThreadModel.generalDictionary, longestWords);
        }

        private static void RunWork(Object model)
        {
            FileInfo fileInfo = model as FileInfo;
            if (fileInfo != null)
            {
                DoWork(fileInfo);
            }
            else
            {
                throw new ArgumentNullException("Passing model is not ThreadModel");
            }
        }

        private static void RunWorkThread(Object model)
        {
            while (fileInfos.Count != 0)
            {
                lock (fileInfos)
                {
                    FileInfo fileInfo;
                    fileInfos.TryPop(out fileInfo);
                    if (fileInfo != null)
                    {
                        DoWork(fileInfo);
                    }
                }
            }
            
        }
    }
}

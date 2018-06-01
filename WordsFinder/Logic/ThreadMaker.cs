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
        private Func<Object> threadFunc;
        private static Stack<FileInfo> fileInfos;
        private int countOfThreads;
        public static object locker = new object();

        public ThreadMaker(Stack<FileInfo> fileInfos, int countOfThreads)
        {
            ThreadMaker.fileInfos = fileInfos;
            this.countOfThreads = countOfThreads;
        }

        public void MakeThreads()
        {
            for (int i = 0; i < countOfThreads; i++)
            {
                Thread thread = new Thread(RunWorkThread);
                thread.IsBackground = true;
                thread.Start(fileInfos.Pop());
            }
        }

        public void MakeThreadpoolThreads()
        {
            while (fileInfos.Count != 0)
            {
                ThreadPool.QueueUserWorkItem(RunWork, fileInfos.Pop());
                ThreadPool.SetMaxThreads(countOfThreads, countOfThreads*2);
            }
        }

        public void MakeParallelLoopThreads()
        {
            ParallelLoopResult result = Parallel.ForEach<FileInfo>(fileInfos,DoWork);
        }

        public static void DoWork(FileInfo fileInfo)
        {
            
            ThreadModel threadModel = new ThreadModel(fileInfo);

            String text = File.ReadAllText(threadModel.fileInfo.FullName);

            Console.WriteLine(text);

            lock (locker)
            {
                Dictionary<string, int> longestWords = WordFinder.FindLongestWord(text, 10);
                DictionaryUpdater.MergeTwoDictionary(ref ThreadModel.generalDictionary, longestWords);
            }
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
            FileInfo fileInfo = model as FileInfo;
            if (fileInfo != null)
            {
                DoWork(fileInfo);
                lock (fileInfos)
                {
                    if (fileInfos.Count != 0)
                    {
                        Thread thread = new Thread(RunWorkThread);
                        thread.Start(fileInfos.Pop());
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("Passing model is not ThreadModel");
            }
            
        }
        
    }
}

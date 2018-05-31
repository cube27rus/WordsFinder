using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace WordsFinder.Logic
{
    public class ThreadMaker
    {
        public static ConcurrentStack<Thread> threadStack;
        private Func<Object> threadFunc;
        private Stack<FileInfo> fileInfos;
        private int countOfThreads;
        public static object locker = new object();

        public ThreadMaker(Stack<FileInfo> fileInfos, int countOfThreads)
        {
            this.fileInfos = fileInfos;
            this.countOfThreads = countOfThreads;
        }

        public void MakeThreads()
        {
            while (fileInfos.Count != 0)
            {
                for (int i = 0; i < countOfThreads; i++)
                {
                    ThreadModel threadModel = new ThreadModel(fileInfos.Pop());
                    ThreadPool.QueueUserWorkItem(DoWork, threadModel);
                }

                Thread.Sleep(100);
            }

            Thread.CurrentThread.Interrupt();
        }

        public static void DoWork(Object model)
        {
            ThreadModel threadModel = model as ThreadModel;
            if (threadModel != null)
            {
                String text = File.ReadAllText(threadModel.fileInfo.FullName);
                
                Dictionary<string, int> longestWords = WordFinder.FindLongestWord(text, 10);

                lock (locker)
                {
                    DictionaryUpdater.MergeTwoDictionary(ref ThreadModel.generalDictionary, longestWords);
                }
            }
            else
            {
                throw new ArgumentNullException("Passing model is not ThreadModel");
            }
        }
    }
}

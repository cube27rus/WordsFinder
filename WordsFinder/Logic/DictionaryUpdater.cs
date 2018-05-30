using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsFinder.Logic
{
    public class DictionaryUpdater
    {
        public static void MergeTwoDictionary(ref Dictionary<string, int> generalDictionary, Dictionary<string, int> sourceDictionary)
        {
            foreach (KeyValuePair<string, int> entry in sourceDictionary)
            {
                if (generalDictionary.ContainsKey(entry.Key))
                {
                    generalDictionary[entry.Key] += entry.Value;
                }
                else
                {
                    generalDictionary.Add(entry.Key, entry.Value);
                }
            }
        }

        public static Dictionary<T, T1> SaveTenLargestValues<T, T1>(Dictionary<T, T1> refDictionary)
        {
           return refDictionary.OrderByDescending(r => r.Value).Take(10).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}

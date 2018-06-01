using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordsFinder.Logic
{
    public static class DictionaryUpdater
    {
        public static void MergeTwoDictionary(ref Dictionary<string, int> generalDictionary, Dictionary<string, int> sourceDictionary)
        {
            if (generalDictionary.Count == 0)
            {
                generalDictionary = sourceDictionary;
            }
            else
            {
                List<String> usedKey = new List<string>();

                foreach (KeyValuePair<string, int> entry in sourceDictionary)
                {
                    if (generalDictionary.ContainsKey(entry.Key))
                    {
                        generalDictionary[entry.Key] += entry.Value;
                        usedKey.Add(entry.Key);
                    }
                }
                
                for (int i = generalDictionary.Count - 1; i >= 0; i--)
                {
                    var item = generalDictionary.ElementAt(i);
                    var itemKey = item.Key;
                    if (!usedKey.Contains(itemKey))
                    {
                        generalDictionary.Remove(itemKey);
                    }
                }
                
                if (generalDictionary.Count == 0)
                {
                    throw new Exception("Нет повторяющихся слов в файлах");
                }

            }
        }

        public static Dictionary<T, T1> SaveTenLargestValues<T, T1>(this Dictionary<T, T1> refDictionary)
        {
           return refDictionary.OrderByDescending(r => r.Value).Take(10).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}

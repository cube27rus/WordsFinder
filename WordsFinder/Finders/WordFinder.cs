using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WordsFinder.Logic
{
    public class WordFinder
    {
        public static Dictionary<string, int> FindLongestWord(string text, int length)
        {
            Dictionary<string, int> countWords = new Dictionary<string, int>();
            Regex regex = new Regex("\\b\\w{" + length + ",}\\b");

            foreach (Match ItemMatch in regex.Matches(text))
            {
                string item = ItemMatch.ToString();
                if (countWords.ContainsKey(item))
                {
                    countWords[item]++;
                }
                else
                {
                    countWords.Add(item,1);
                }
                
            }
            countWords.SaveTenLargestValues();
            return countWords;  
        }
    }
}

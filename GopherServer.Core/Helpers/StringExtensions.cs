using System;
using System.Collections.Generic;
using System.Linq;
using GopherServer.Core.Models;

namespace GopherServer.Core.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Replaces characters that stuff our clients up
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CleanString(this string text)
        {
            return text.Replace("\r", "")
                       .Replace("\t", "")
                       .Replace("\n", "")
                       .Trim();
        }

        /// <summary>
        /// Wraps text to the specified column length
        /// </summary>
        /// <param name="text"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static List<string> WrapText(this string text, int cols = 80)
        {
            var words = text.Split(new string[] { " ", "\r\n", "\n" }, StringSplitOptions.None);

            List<string> wordList = new List<string>();

            string line = "";
            foreach (string word in words)
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    var newLine = string.Join(" ", line, word).Trim();
                    if (newLine.Length >= cols)
                    {
                        wordList.Add(line);
                        line = word;
                    }
                    else
                    {
                        line = newLine;
                    }
                }
            }

            if (line.Length > 0)
                wordList.Add(line);

            return wordList;
        }

        /// <summary>
        /// Wraps the specified string into DirectoryItem Info lines
        /// </summary>
        /// <param name="text"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static List<DirectoryItem> WrapToDirectoryItems(this string text, int cols = 80)
        {
            return text.WrapText(cols).Select(l => new DirectoryItem(l)).ToList();
        }
    }
}

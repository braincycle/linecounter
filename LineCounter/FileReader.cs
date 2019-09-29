using System;
using Linecounter.Params;
using System.Collections.Generic;
using System.Globalization;
using Linecounter.Logger.Abstract;
using System.IO;
using System.Linq;

namespace Linecounter
{
    public class FileReader
    {
        private IDictionary<int, string> _defectiveStrings;
        private IDictionary<int, string> _matchingStrings;
        private readonly NumberStyles _style = NumberStyles.AllowDecimalPoint;
        private IFileLogger _fileLogger;

        public FileReader(IFileLogger fileLogger)
        {
            _fileLogger = fileLogger;
            _defectiveStrings = new Dictionary<int, string>();
            _matchingStrings = new Dictionary<int, string>();
        }

        public void RunOptions(Options opts)
        {
            IEnumerable<string> files = opts.InputFiles;
            ParseFiles(files);
        }

        void ShowResultInFile(string file, IDictionary<int, string> matchingStrings, IDictionary<int, string> defectiveStrings)
        {
            if ((matchingStrings.Count == 0) && (defectiveStrings.Count == 0))
            {
                if (_fileLogger != null)
                {
                    _fileLogger.ShowEmptyFileMessage(file);
                    return;
                }

                Console.WriteLine($"No matching strings found or defective strings found in the file {file}");
                return;
            }
            else
            {
                WriteMatchStrings(matchingStrings);
                WriteDefectiveStrings(defectiveStrings);

                if (matchingStrings.Count > 0)
                    WriteMaxSumLine(matchingStrings);
            }
        }

        void ParseFiles(IEnumerable<string> files)
        {
            if ((files != null) && (files.Count() != 0))
            {
                string[] lines = null;

                foreach (var file in files)
                {
                    try
                    {
                        if (_fileLogger != null)
                        {
                            _fileLogger.ShowFileNameMessage(file);
                        }
                        else
                        {
                            Console.WriteLine($"--------------File {file}");
                        }

                        lines = File.ReadAllLines(@"" + file);

                        ParseLines(lines);

                        ShowResultInFile(file, _matchingStrings, _defectiveStrings);

                        ClearAllCollections(_matchingStrings, _defectiveStrings);
                    }
                    catch (Exception ex)
                    {
                        if (_fileLogger != null)
                        {
                            _fileLogger.ShowFileNotFoundMessage(file);
                        }
                        else
                        {
                            Console.WriteLine($"Error: the file {file} not found...");
                        }
                        //throw ex; -> if you need
                    }
                }
            }
            else
            {
                Console.WriteLine("Error parsing files...Сheck the command line options you passed or read the app manual");
            }
        }

        void ParseLines(string[] lines)
        {
            var index = 1;
            foreach (var line in lines)
            {
                string[] words = SplitLine(line);

                if (IsMatchStr(words))
                {
                    CollectMatchingString(index, line);
                }
                else
                {
                    CollectDefectiveString(index, line);
                }

                index++;
            }
        }

        void CollectMatchingString(int index, string line)
        {
            _matchingStrings.Add(index, line);
        }

        void CollectDefectiveString(int index, string line)
        {
            _defectiveStrings.Add(index, line);
        }

        string[] SplitLine(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                return line.Split(',');
            }

            return new string[] { };
        }

        void WriteMaxSumLine(IDictionary<int, string> matchingStrings)
        {
            if (_fileLogger != null)
            {
                _fileLogger.ShowMaxSumMessage();
            }
            else
            {
                Console.WriteLine("-----Line with the maximum amount of elements:");
            }

            var sumDictionary = new Dictionary<int, double>();
            var sum = 0d;

            foreach (var str in matchingStrings)
            {
                sum = StrConvert(str.Value);
                sumDictionary.Add(str.Key, sum);

                if (matchingStrings.LastOrDefault().Equals(str))
                {
                    foreach (var element in sumDictionary)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"sum of elements in the {element.Key} line = { element.Value}");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
            }

            var maxElem = sumDictionary.OrderBy(x => x.Value).Last();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Line with max sum of elements = {maxElem.Key}, sum of elements = {maxElem.Value} ");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public double StrConvert(string str)
        {
            string[] str_arr = str.Split(',');
            double sum = 0;

            for (int i = 0; i < str_arr.Length; i++)
            {
                sum += double.Parse(str_arr[i]);
            }
            return sum;
        }

        void ClearAllCollections(IDictionary<int, string> matchingStrings, IDictionary<int, string> defectiveStrings)
        {
            if ((matchingStrings != null) && (matchingStrings.Count > 0)) matchingStrings.Clear();
            if ((defectiveStrings != null) && (defectiveStrings.Count > 0)) defectiveStrings.Clear();
        }

        public void WriteMatchStrings(IDictionary<int, string> matchingStrings)
        {
            if (_fileLogger != null)
            {
                _fileLogger.ShowMatchingLinesMessage();
            }
            else
            {
                Console.WriteLine("----Matching strings:");
            }

            foreach (var str in matchingStrings)
            {
                Console.WriteLine("Line: {0}, Value: {1}", str.Key, str.Value);
            }

            if (matchingStrings.Count == 0) Console.WriteLine($"\tNo matching strings found!");
        }

        public void WriteDefectiveStrings(IDictionary<int, string> defectiveStrings)
        {
            if (_fileLogger != null)
            {
                _fileLogger.ShowDefectiveLinesMessage();
            }
            else
            {
                Console.WriteLine("----Defective strings:");
            }

            foreach (var str in defectiveStrings)
            {
                Console.WriteLine("Line: {0}, Value: {1}", str.Key, str.Value);
            }

            if (defectiveStrings.Count == 0) Console.WriteLine($"\tNo defective strings found!");
        }

        public bool IsMatchStr(string[] words)
        {
            var isMatch = true;
            if (words.Length > 0)
            {
                foreach (var word in words)
                {
                    if (IsNumber(word) == false)
                    {
                        isMatch = false;
                        break;
                    }
                }
            }

            return isMatch;
        }

        public bool IsNumber(string word)
        {
            decimal number;
            try
            {
                if (Decimal.TryParse(word, out number)) return true;

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Parsing error of word: {word}");
                throw ex;
            }
        }
    }
}
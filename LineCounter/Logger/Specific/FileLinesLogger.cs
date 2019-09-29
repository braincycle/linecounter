using System;
using Linecounter.Logger.Abstract;

namespace Linecounter.Logger.Specific
{
    class FileLinesLogger : IFileLogger
    {
        public void ShowEmptyFileMessage(string filePath)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"No matching strings found or defective strings found in the file {filePath}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void ShowFileNameMessage(string filePath)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n-------------- File {filePath} --------------\n");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void ShowMaxSumMessage()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Line with the maximum amount of elements in this file:");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void ShowMatchingLinesMessage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Matching strings in this file:");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void ShowDefectiveLinesMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Defective strings in this file:");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void ShowFileNotFoundMessage(string filePath)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: the file {filePath} not found...");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
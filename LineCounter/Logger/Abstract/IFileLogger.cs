using System;
using System.Collections.Generic;
using System.Text;

namespace Linecounter.Logger.Abstract
{
    interface IFileLogger
    {
        void ShowFileNameMessage(string filePath);
        void ShowEmptyFileMessage(string filePath);
        void ShowMatchingLinesMessage();
        void ShowDefectiveLinesMessage();
        void ShowFileNotFoundMessage(string filePath);
        void ShowMaxSumMessage();
    }
}
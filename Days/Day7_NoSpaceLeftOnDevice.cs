using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2022.Days {
    internal class Day7_NoSpaceLeftOnDevice : IAoCTask {
        public static string SolvePart1(string inputFilePath) {
            var inputData = File.ReadLines(inputFilePath);
            int maxAcceptedFileSize = 100000;
            CommunicationDevice commDevice = new(inputData);

            int fileSizeSum = commDevice.GetSumOfAllDirectoriesTotalFileSizeWithLimit(maxAcceptedFileSize); ;
            Debug.Assert(fileSizeSum == 1749646);
            return $"Sum of the total sizes of those directories with a lower file size than {maxAcceptedFileSize} is {fileSizeSum}";
        }

        public static string SolvePart2(string inputFilePath) {
            var inputData = File.ReadLines(inputFilePath);
            int totalDiskSpace = 70000000;
            int updateSize = 30000000;

            CommunicationDevice commDevice = new(inputData, totalDiskSpace);
            int freedFileSize = commDevice.FindAndDeleteDirectoryToFreeUpEnoughDiskSpace(updateSize);
            Debug.Assert(freedFileSize == 1498966);
            return $"Total size of the deleted directory to free up disk space for a update is {freedFileSize}";
        }
    }
}

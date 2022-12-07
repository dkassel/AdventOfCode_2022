using System.Diagnostics;

namespace AdventOfCode_2022.Days {
    internal class Day6_TuningTrouble : IAoCTask {
        public static string SolvePart1(string inputFilePath) {
            List<string> dataStreams = File.ReadLines(inputFilePath).ToList();
            int startOfPacketMarker = CommunicationDevice.FindStartOfPacketMarker(dataStreams.First(), 4);
            Debug.Assert(startOfPacketMarker == 1198);
            return $"Start of message index for 7 distinct characters is at index {startOfPacketMarker}";
        }

        public static string SolvePart2(string inputFilePath) {
            List<string> dataStreams = File.ReadLines(inputFilePath).ToList();
            int startOfPacketMarker = CommunicationDevice.FindStartOfPacketMarker(dataStreams.First(), 14);
            Debug.Assert(startOfPacketMarker == 3120);
            return $"Start of message index for 14 distinct characters is at index {startOfPacketMarker}";
        }

    }
}

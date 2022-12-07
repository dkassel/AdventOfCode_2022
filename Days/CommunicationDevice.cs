using System.Diagnostics;

namespace AdventOfCode_2022.Days {
    internal record class TreeNode {
        public string Name { get; init; }
        public Directory? Parent { get; init; }

        public TreeNode(string name, Directory? parent) { Name = name; Parent = parent; }
    }

    internal record class Directory : TreeNode {
        public Directory(string name, Directory? parent) : base(name, parent) { }
        public List<Directory> SubDirectories { get; init; } = new();
        public List<PlainData> PlainDataFiles { get; init; } = new();

        public Directory? FindDirectoryByName(string directoryName) {
            Directory? foundDir = SubDirectories.Find((subDir) => subDir.Name == directoryName);
            if (foundDir is not null) return foundDir;

            else if (SubDirectories.Count > 0) {
                foreach (Directory dir in SubDirectories) {
                    foundDir = FindDirectoryByName(directoryName);
                    if (foundDir is not null) return foundDir;
                }
            }
            return null;
        }

        public List<Directory> GetAllSubDirs() {
            List<Directory> subDirs = new() {
                this
            };
            if (SubDirectories.Count > 0) {
                foreach (Directory dir in SubDirectories) {
                    subDirs.AddRange(dir.GetAllSubDirs());
                }
            }
            return subDirs;
        }

        public List<PlainData> GetAllSubFiles() {
            List<PlainData> subFiles = new();
            subFiles.AddRange(this.PlainDataFiles);
            if (SubDirectories.Count > 0) {
                foreach (Directory dir in SubDirectories) {
                    subFiles.AddRange(dir.GetAllSubFiles());
                }
            }
            return subFiles;
        }

        public int TotalFileSizeSumOfDirectory() {
            var subFiles = GetAllSubFiles();
            return subFiles.Select((file) => file.DataSize).Sum();
        }

        public override string ToString() {
            return Name;
        }
    }

    internal record class PlainData : TreeNode {
        public int DataSize { get; init; }
        public PlainData(string name, Directory? parent, int dataSize) : base(name, parent) { DataSize = dataSize; }
        public override string ToString() {
            return Name + "/" + DataSize;
        }
    }

    internal class CommunicationDevice {
        public CommunicationDevice(IEnumerable<string> terminalOutput, int totalAvailaibleDiskSpace = 0) {
            TotalAvailaibleDiskSpace = totalAvailaibleDiskSpace;
            DataTree = BuildTreeFromTerminalOutputData(terminalOutput);
        }

        public int TotalAvailaibleDiskSpace { get; }
        private List<TreeNode> DataTree { get; set; } = new();

        public static int FindStartOfPacketMarker(string dataStream, int nrOfDistinctCharacters) {
            string uniqueChars = "";
            for (int i = 0; i < dataStream.Length; i++) {
                if (uniqueChars.Length == nrOfDistinctCharacters)
                    return i;
                else if (uniqueChars.Contains(dataStream[i]))
                    uniqueChars = uniqueChars[(uniqueChars.IndexOf(dataStream[i]) + 1)..];
                uniqueChars += dataStream[i];
            }
            return -1;
        }

        public static int FindStartOfPacketMarkerWithHashSet(string dataStream, int nrOfDistinctCharacters) {
            HashSet<char> distinctChars = new();
            for (int i = 0; i < dataStream.Length; i++) {
                distinctChars.UnionWith(dataStream[i..(i + nrOfDistinctCharacters)]);
                if (distinctChars.Count == nrOfDistinctCharacters)
                    return i + nrOfDistinctCharacters;
                distinctChars.Clear();
            }
            return -1;
        }

        public static void PerformanceTestForStringAndHashSet(string dataStream, int nrOfDistinctCharacters) {
            Stopwatch stopwatch = new();
            stopwatch.Restart();
            _ = FindStartOfPacketMarker(dataStream, nrOfDistinctCharacters);
            stopwatch.Stop();
            Debug.WriteLine($"Time with string {stopwatch.ElapsedTicks}");
            stopwatch.Restart();
            stopwatch.Start();
            _ = FindStartOfPacketMarkerWithHashSet(dataStream, nrOfDistinctCharacters);
            stopwatch.Stop();
            Debug.WriteLine($"Time with Hashset {stopwatch.ElapsedTicks}");

            stopwatch.Restart();
            _ = FindStartOfPacketMarker(dataStream, nrOfDistinctCharacters);
            stopwatch.Stop();
            Debug.WriteLine($"Time with string {stopwatch.ElapsedTicks}");
            stopwatch.Restart();
            stopwatch.Start();
            _ = FindStartOfPacketMarkerWithHashSet(dataStream, nrOfDistinctCharacters);
            stopwatch.Stop();
            Debug.WriteLine($"Time with Hashset {stopwatch.ElapsedTicks}");

            stopwatch.Restart();
            _ = FindStartOfPacketMarker(dataStream, nrOfDistinctCharacters);
            stopwatch.Stop();
            Debug.WriteLine($"Time with string {stopwatch.ElapsedTicks}");
            stopwatch.Restart();
            stopwatch.Start();
            _ = FindStartOfPacketMarkerWithHashSet(dataStream, nrOfDistinctCharacters);
            stopwatch.Stop();
            Debug.WriteLine($"Time with Hashset {stopwatch.ElapsedTicks}");
        }

        public int GetSumOfAllDirectoriesTotalFileSizeWithLimit(int directorySizeLimit) {
            List<int> directorySizes = new();
            Directory root = (Directory)DataTree.First();

            foreach (Directory dir in root.GetAllSubDirs()) {
                directorySizes.Add(dir.TotalFileSizeSumOfDirectory());
            }
            return directorySizes.Where((dirSize) => dirSize <= directorySizeLimit).Sum();
        }

        public int FindAndDeleteDirectoryToFreeUpEnoughDiskSpace(int diskSpaceSizeThatNeedsToBeAvailaibleAfter) {
            Directory root = (Directory)DataTree.First();
            int currentFreeDiskSpace = TotalAvailaibleDiskSpace - root.TotalFileSizeSumOfDirectory();
            List<int> dirSizes = new();
            foreach (Directory dir in root.GetAllSubDirs()) {
                int dirSize = dir.TotalFileSizeSumOfDirectory();
                if (dirSize + currentFreeDiskSpace >= diskSpaceSizeThatNeedsToBeAvailaibleAfter)
                    dirSizes.Add(dirSize);
            }
            return dirSizes.Min();
        }


        private static List<TreeNode> BuildTreeFromTerminalOutputData(IEnumerable<string> terminalOutput) {
            const char commandSymbol = '$';
            const string changeDirectoryCommand = "cd";
            const string directorySymbol = "dir";
            const string rootDirectorySymbol = "/";

            List<TreeNode> dataTree = new();
            Directory rootDir = new(rootDirectorySymbol, null);
            dataTree.Add(rootDir);
            Directory currentDirectory = rootDir;

            bool isDirectory(string terminalOutputLine) => terminalOutputLine.StartsWith(directorySymbol);
            bool isFile(string terminalOutputLine) => !terminalOutputLine.StartsWith(directorySymbol) && !terminalOutputLine.StartsWith(commandSymbol);
            bool isCommandSymbol(string terminalOutputLine) => terminalOutputLine.StartsWith(commandSymbol);
            bool isChangeDirCommand(string terminalOutputLine) => terminalOutputLine.StartsWith(changeDirectoryCommand);

            void executeChangeDirectoryCommand(string outputLine) {
                outputLine = outputLine[(changeDirectoryCommand.Length + 1)..];
                if (outputLine.StartsWith(rootDirectorySymbol)) {
                    currentDirectory = rootDir;
                }
                else if (outputLine.StartsWith("..") && currentDirectory.Parent != null) {
                    currentDirectory = currentDirectory.Parent;
                }
                else {
                    Directory? dir = currentDirectory.SubDirectories.Find((dir) => dir.Name == outputLine);
                    if (dir is null) {
                        dir = new(outputLine, currentDirectory);
                        currentDirectory.SubDirectories.Add(dir);
                    }
                    currentDirectory = dir;
                }
            }

            foreach (string outputLine in terminalOutput) {
                if (isCommandSymbol(outputLine)) {
                    string shortenedOutputLine = outputLine[2..];
                    if (isChangeDirCommand(shortenedOutputLine)) {
                        executeChangeDirectoryCommand(shortenedOutputLine);
                    }
                }
                else {
                    string[] lineData = outputLine.Split(" ");
                    if (isDirectory(outputLine)) {
                        string dirName = lineData[1];
                        if (currentDirectory.SubDirectories.Find((dir) => dir.Name == dirName) is null) {
                            currentDirectory.SubDirectories.Add(new Directory(dirName, currentDirectory));
                        }
                    }
                    else if (isFile(outputLine)) {
                        int dataFileSize = int.Parse(lineData[0]);
                        string dataFileName = lineData[1];
                        if (currentDirectory.PlainDataFiles.Find((dataFile) => dataFile.Name == dataFileName) is null) {
                            currentDirectory.PlainDataFiles.Add(new PlainData(dataFileName, currentDirectory, dataFileSize));
                        }
                    }
                }
            }

            return dataTree;
        }
    }
}

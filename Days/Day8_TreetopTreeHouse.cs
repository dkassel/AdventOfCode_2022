using System.Diagnostics;

namespace AdventOfCode_2022.Days {
    internal class Day8_TreetopTreeHouse : IAoCTask {
        public static string SolvePart1(string inputFilePath) {
            List<string> inputData = File.ReadLines(inputFilePath).ToList();
            int nrOfVisibleTrees = GetNumberOfTreesThatAreVisibleFromOutsideTheGrid(inputData);
            Debug.Assert(nrOfVisibleTrees == 1698);
            return $"{nrOfVisibleTrees} tree/s are visible from the outside of the forest area/grid.";
        }

        public static string SolvePart2(string inputFilePath) {
            List<string> inputData = File.ReadLines(inputFilePath).ToList();
            int highestScenicScore = GetHighestPossibleScenicScore(inputData);
            Debug.Assert(highestScenicScore == 672280);
            return $"The highest scenic score for a tree in the given area is {highestScenicScore}";
        }

        private static int CharToInt(char charToConvert) => int.Parse(charToConvert.ToString());

        private static int GetNumberOfTreesThatAreVisibleFromOutsideTheGrid(List<string> treeMap) {
            int nrOfVisibleInteriorTrees = 0;
            int treeRowLength = treeMap.First().Length;
            int treeColumnLength = treeMap.Count;
            int nrOfEdgeTrees = treeRowLength * 2 + treeColumnLength * 2 - 4;

            for (int yPos = 1; yPos < treeColumnLength - 1; yPos++) {
                for (int xPos = 1; xPos < treeRowLength - 1; xPos++) {
                    if (IsVisibleFromLeft(treeMap, xPos, yPos) || IsVisibleFromRight(treeMap, xPos, yPos) ||
                        IsVisibleFromTop(treeMap, xPos, yPos) || IsVisibleFromBottom(treeMap, xPos, yPos)) {
                        nrOfVisibleInteriorTrees++;
                    }
                }
            }
            return nrOfVisibleInteriorTrees + nrOfEdgeTrees;
        }

        private static bool NoHigherTreeOnTheLeft(string treeRow, int treeIndexInRow) {
            if (treeIndexInRow == 0) return true;
            int treeHeight = CharToInt(treeRow[treeIndexInRow]);

            for (int xIter = treeIndexInRow - 1; xIter >= 0; xIter--) {
                int otherTreeHeight = CharToInt(treeRow[xIter]);
                if (otherTreeHeight >= treeHeight) return false;
            }
            return true;
        }

        private static bool NoHigherTreeOnTheRight(string treeRow, int treeIndexInRow) {
            if (treeIndexInRow == treeRow.Length - 1) return true;
            int treeHeight = CharToInt(treeRow[treeIndexInRow]);

            for (int xIter = treeIndexInRow + 1; xIter < treeRow.Length; xIter++) {
                int otherTreeHeight = CharToInt(treeRow[xIter]);
                if (otherTreeHeight >= treeHeight) return false;
            }
            return true;
        }

        private static bool IsVisibleFromLeft(List<string> treeMap, int treeXPos, int treeYPos) => NoHigherTreeOnTheLeft(treeMap[treeYPos], treeXPos);
        private static bool IsVisibleFromRight(List<string> treeMap, int treeXPos, int treeYPos) => NoHigherTreeOnTheRight(treeMap[treeYPos], treeXPos);
        private static bool IsVisibleFromTop(List<string> treeMap, int treeXPos, int treeYPos) => NoHigherTreeOnTheLeft(string.Concat(treeMap.Select((treeRow) => treeRow[treeXPos])), treeYPos);
        private static bool IsVisibleFromBottom(List<string> treeMap, int treeXPos, int treeYPos) => NoHigherTreeOnTheRight(string.Concat(treeMap.Select((treeRow) => treeRow[treeXPos])), treeYPos);

        private static int GetHighestPossibleScenicScore(List<string> treeMap) {
            int xLength = treeMap.First().Length;
            int yLength = treeMap.Count;

            List<int> scenicScores = new();
            for (int yPos = 1; yPos < yLength - 1; yPos++) {
                for (int xPos = 1; xPos < xLength - 1; xPos++) {
                    scenicScores.Add(GetScenicScoreOfTree(treeMap, xPos, yPos));
                }
            }
            return scenicScores.Max();
        }

        private static int GetScenicScoreOfTree(List<string> treeMap, int xPos, int yPos) {
            int leftScore = HowFarCanYouLookToTheLeft(treeMap, xPos, yPos);
            int rightScore = HowFarCanYouLookToTheRight(treeMap, xPos, yPos);
            int topScore = HowFarCanYouLookToTheTop(treeMap, xPos, yPos);
            int bottomScore = HowFarCanYouLookToTheBottom(treeMap, xPos, yPos);
            return leftScore * rightScore * topScore * bottomScore;
        }

        private static int HowFarCanYouLookToTheLeft(string currentRow, int currentXPos) {
            int treeHeight = CharToInt(currentRow[currentXPos]);
            int nrOfVisibleTrees = 0;
            for (int xIter = currentXPos - 1; xIter >= 0; xIter--) {
                nrOfVisibleTrees++;
                int otherTreeHeight = CharToInt(currentRow[xIter]);
                if (otherTreeHeight >= treeHeight) break;
            }
            return nrOfVisibleTrees;
        }

        private static int HowFarCanYouLookToTheRight(string currentRow, int currentXPos) {
            int treeHeight = CharToInt(currentRow[currentXPos]);
            int nrOfVisibleTrees = 0;
            for (int xIter = currentXPos + 1; xIter < currentRow.Length; xIter++) {
                nrOfVisibleTrees++;
                int otherTreeHeight = CharToInt(currentRow[xIter]);
                if (otherTreeHeight >= treeHeight) break;
            }
            return nrOfVisibleTrees;
        }

        private static int HowFarCanYouLookToTheLeft(List<string> treeMap, int currentXPos, int currentYPos) => HowFarCanYouLookToTheLeft(treeMap[currentYPos], currentXPos);
        private static int HowFarCanYouLookToTheRight(List<string> treeMap, int currentXPos, int currentYPos) => HowFarCanYouLookToTheRight(treeMap[currentYPos], currentXPos);
        private static int HowFarCanYouLookToTheTop(List<string> treeMap, int currentXPos, int currentYPos) => HowFarCanYouLookToTheLeft(string.Concat(treeMap.Select((treeRow) => treeRow[currentXPos])), currentYPos);
        private static int HowFarCanYouLookToTheBottom(List<string> treeMap, int currentXPos, int currentYPos) => HowFarCanYouLookToTheRight(string.Concat(treeMap.Select((treeRow) => treeRow[currentXPos])), currentYPos);
    }
}

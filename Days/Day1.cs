namespace AdventOfCode_2022.Days
{
    internal class Day1
    {

        private static List<int> GetCalorieSumsOfAllElfs(List<string> input)
        {
            List<int> calorieSums = new();
            int currentCalory = 0;
            foreach (string line in input)
            {
                if (int.TryParse(line, out int calory))
                    currentCalory += calory;
                else
                {
                    calorieSums.Add(currentCalory);
                    currentCalory = 0;
                }
            }
            if (currentCalory > 0) calorieSums.Add(currentCalory);
            return calorieSums;
        }

        public static string SolvePart1(string inputFilePath)
        {
            List<int> caloryList = GetCalorieSumsOfAllElfs(System.IO.File.ReadLines(inputFilePath).ToList());
            return $"Max calories of one elf are {caloryList.Max()}";
        }

        public static string SolvePart2(string inputFilePath)
        {
            List<int> caloryList = GetCalorieSumsOfAllElfs(System.IO.File.ReadLines(inputFilePath).ToList());
            caloryList = caloryList.OrderByDescending(i => i).ToList();
            var top3Calories = caloryList.Take(3);

            return $"Max calories of top 3 elf are {top3Calories.Sum()}";
        }

    }
}

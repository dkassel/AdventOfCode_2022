namespace AdventOfCode_2022.Days
{
    internal class Backpack
    {
        internal Backpack(string firstCompartment, string secondCompartment)
        {
            FirstCompartment = firstCompartment.Select(letter => LetterToNumberValue(letter)).ToList();
            SecondCompartment = secondCompartment.Select(letter => LetterToNumberValue(letter)).ToList();
        }

        public static int LetterToNumberValue(char c) => char.IsLower(c) ? ((short)c) - 96 : ((short)c) - 38;

        public List<int> GetWholeBackpackItemsAsPriorites() => FirstCompartment.Concat(SecondCompartment).ToList();
        public List<int> FirstCompartment { get; }
        public List<int> SecondCompartment { get; }

        public int GetItemTypePrioritySum() => FirstCompartment.Where(letter => SecondCompartment.Contains(letter)).Distinct().ToList().Sum();

    }

    internal class Day3_RucksackReorganization
    {

        private static Backpack StringToBackpack(string backpackContent) => new(backpackContent[..(backpackContent.Length / 2)], backpackContent[(backpackContent.Length / 2)..]);
        public static string SolvePart1(string inputFilePath)
        {
            List<string> fileLines = File.ReadLines(inputFilePath).ToList();
            int sumOfTheItemPrioritiesInBothCompartements = fileLines.Select(line => StringToBackpack(line).GetItemTypePrioritySum()).Sum();
            return $"The sum of the priorities of item types that are in both backpack compartements is {sumOfTheItemPrioritiesInBothCompartements}";
        }

        public static string SolvePart2(string inputFilePath)
        {
            string[] fileLines = File.ReadLines(inputFilePath).ToArray();
            int sumOfTheBadgePriorities = 0;
            for (int i = 0; i < fileLines.Length - 1; i += 3)
            {
                List<int> backpackContentOf1stElf = fileLines[i].Select(letter => Backpack.LetterToNumberValue(letter)).Distinct().ToList();
                List<int> backpackContentOf2ndElf = fileLines[i + 1].Select(letter => Backpack.LetterToNumberValue(letter)).Distinct().ToList();
                List<int> backpackContentOf3rdElf = fileLines[i + 2].Select(letter => Backpack.LetterToNumberValue(letter)).Distinct().ToList();
                List<int> itemsInBothCompartments = backpackContentOf1stElf.Where(letter => backpackContentOf2ndElf.Contains(letter) && backpackContentOf3rdElf.Contains(letter)).Distinct().ToList();
                sumOfTheBadgePriorities += itemsInBothCompartments.Sum();
            }
            return $"The sum of the priorities badges that are in 3 elfs backpacks is {sumOfTheBadgePriorities}";
        }
    }
}

namespace AdventOfCode_2022.Days
{
    internal class ElfPair
    {
        public List<int> Elf1SectionIds;
        public List<int> Elf2SectionIds;
        internal ElfPair(string inputLine)
        {
            string[] elfPairData = inputLine.Split(',');
            Elf1SectionIds = ElfStringToListOfSectionIds(elfPairData[0]);
            Elf2SectionIds = ElfStringToListOfSectionIds(elfPairData[1]);
        }
        private static List<int> ElfStringToListOfSectionIds(string elfData)
        {
            string[] sectionIdsOfElf = elfData.Split('-');
            int firstSectionId = int.Parse(sectionIdsOfElf[0]);
            int rangeSize = int.Parse(sectionIdsOfElf[1]) - firstSectionId + 1;
            return Enumerable.Range(firstSectionId, rangeSize).ToList();
        }

        public bool DoesTheAssignedRangeOfOneElfContainTheOther()
        {
            int nrOfSharedSectionIds = Elf1SectionIds.Intersect(Elf2SectionIds).Count();
            return nrOfSharedSectionIds == Elf1SectionIds.Count || nrOfSharedSectionIds == Elf2SectionIds.Count;
        }

        public bool DoesTheAssignedRangeOfOneElfOverlapWithTheOther() => Elf1SectionIds.Intersect(Elf2SectionIds).Any();
    }

    internal class Day4_CampCleanup : IAoCTask
    {
        public static string SolvePart1(string inputFilePath)
        {
            var inputStrings = File.ReadLines(inputFilePath);
            var overlaps = inputStrings.Where(line => new ElfPair(line).DoesTheAssignedRangeOfOneElfContainTheOther());
            return $"{overlaps.Count()} elf pairs share a section id range.";
        }

        public static string SolvePart2(string inputFilePath)
        {
            var inputStrings = File.ReadLines(inputFilePath);
            var overlaps = inputStrings.Where(line => new ElfPair(line).DoesTheAssignedRangeOfOneElfOverlapWithTheOther());
            return $"{overlaps.Count()} elf pairs are overlapping their assigned section ids with each other.";
        }


    }
}

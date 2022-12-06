namespace AdventOfCode_2022.Days
{
    internal struct MovementInstructions
    {
        public int FromPosition;
        public int ToPosition;
        public int NrOfCratesToMove;
        internal MovementInstructions(int nrOfMovedCrates, int fromPosition, int toPosition)
        {
            NrOfCratesToMove = nrOfMovedCrates;
            FromPosition = fromPosition;
            ToPosition = toPosition;
        }
    }

    internal enum CrateMover
    {
        Model3000,
        Model3001,
    }

    internal class Day5_SupplyStacks
    {
        public static string SolvePart1(string inputFilePath)
        {
            string nameOfTheCratesOnTopOfTheStacks = StartTheCrateMover(File.ReadLines(inputFilePath).ToList(), CrateMover.Model3000);
            return $"Crates on top after they were reordered with the CrateMover 3000: {nameOfTheCratesOnTopOfTheStacks}";
        }
        public static string SolvePart2(string inputFilePath)
        {
            string nameOfTheCratesOnTopOfTheStacks = StartTheCrateMover(File.ReadLines(inputFilePath).ToList(), CrateMover.Model3001);
            return $"Crates on top after they were reordered with the CrateMover 3001: {nameOfTheCratesOnTopOfTheStacks}";
        }

        public static void ParseCraneMoverInstructions(List<string> instructionsAsStrings,
            out List<MovementInstructions> craneMovements,
            out List<Stack<char>> listOfCrateStacks)
        {
            craneMovements = new();
            Dictionary<int, Stack<char>> crateStacks = new();
            foreach (var line in instructionsAsStrings)
            {
                if (line.StartsWith("move"))
                {
                    string[] moveData = line.Split(' ');
                    MovementInstructions movementOrder = new(int.Parse(moveData[1]), int.Parse(moveData[3]), int.Parse(moveData[5]));
                    craneMovements.Add(movementOrder);
                }
                else
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == '[')
                        {
                            int positionOfCrate = i / 4;
                            if (!crateStacks.ContainsKey(positionOfCrate))
                                crateStacks.Add(positionOfCrate, new Stack<char>());
                            crateStacks[positionOfCrate].Push(line[i + 1]);
                            i += 2;
                        }
                    }
                }
            }
            listOfCrateStacks = new();
            for (int i = 0; i < crateStacks.Count; i++)
                listOfCrateStacks.Add(new(crateStacks[i]));
        }

        public static string StartTheCrateMover(List<string> instructionsAsStrings, CrateMover crateType)
        {
            ParseCraneMoverInstructions(instructionsAsStrings,
                out List<MovementInstructions> craneMovementInstructions,
                out List<Stack<char>> listOfCrateStacks);

            foreach (var moveInstruction in craneMovementInstructions)
            {
                if (crateType == CrateMover.Model3000)
                {
                    for (int i = moveInstruction.NrOfCratesToMove; i > 0; i--)
                    {
                        char crateThatGetsMoved = listOfCrateStacks.ElementAt(moveInstruction.FromPosition - 1).Pop();
                        Stack<char> toStack = listOfCrateStacks.ElementAt(moveInstruction.ToPosition - 1);
                        toStack.Push(crateThatGetsMoved);
                    }
                }
                else if (crateType == CrateMover.Model3001)
                {
                    Stack<char> cratesThatAreMoved = new();
                    Stack<char> fromStack = listOfCrateStacks.ElementAt(moveInstruction.FromPosition - 1);
                    Stack<char> toStack = listOfCrateStacks.ElementAt(moveInstruction.ToPosition - 1);
                    for (int i = moveInstruction.NrOfCratesToMove; i > 0; i--)
                    {
                        cratesThatAreMoved.Push(fromStack.Pop());
                    }
                    while (cratesThatAreMoved.Count > 0)
                        toStack.Push(cratesThatAreMoved.Pop());
                }
            }
            return string.Concat(listOfCrateStacks.Select(stack => stack.First()));
        }

    }
}

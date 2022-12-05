namespace AdventOfCode_2022.Days
{
    internal class Day2
    {
        private static readonly int RockScore = 1;
        private static readonly int PaperScore = 2;
        private static readonly int ScissorsScore = 3;

        private static readonly int ScoreForAWin = 6;
        private static readonly int ScoreForALoss = 0;
        private static readonly int ScoreForADraw = 3;

        enum Move
        {
            Rock,
            Paper,
            Scissors
        }

        enum GameResult
        {
            Win,
            Loose,
            Draw
        }

        private readonly struct GameRound
        {
            public readonly Move yourMove;
            public readonly Move enemyMove;
            public GameRound(char enemyMove, char yourMove)
            {
                this.yourMove = yourMove == 'X' ? Move.Rock : (yourMove == 'Y' ? Move.Paper : Move.Scissors);
                this.enemyMove = enemyMove == 'A' ? Move.Rock : (enemyMove == 'B' ? Move.Paper : Move.Scissors);
            }
        }

        private readonly struct GameRoundForPart2
        {
            public readonly GameResult gameResult;
            public readonly Move enemyMove;
            public GameRoundForPart2(char enemyMove, char gameResult)
            {
                this.gameResult = gameResult == 'X' ? GameResult.Loose : (gameResult == 'Y' ? GameResult.Draw : GameResult.Win);
                this.enemyMove = enemyMove == 'A' ? Move.Rock : (enemyMove == 'B' ? Move.Paper : Move.Scissors);
            }
        }

        private static GameRound StringToGameRound(string commands) => new(commands[0], commands.LastOrDefault());
        private static GameRoundForPart2 StringToGameRoundPart2(string commands) => new(commands[0], commands.LastOrDefault());

        public static string SolvePart1(string inputFilePath)
        {
            List<string> commands = System.IO.File.ReadLines(inputFilePath).ToList();
            int totalScoreOfYou = 0;
            foreach (string command in commands)
                totalScoreOfYou += CalculcateYourScoreOfOneGameRound(StringToGameRound(command));
            return $"Your total score will be {totalScoreOfYou}";
        }


        private static int CalculcateYourScoreOfOneGameRound(GameRound gameRound)
        {
            var enemyMove = gameRound.enemyMove;

            if (gameRound.yourMove == Move.Rock)
                return RockScore + (enemyMove == Move.Rock ? ScoreForADraw : (enemyMove == Move.Paper ? ScoreForALoss : ScoreForAWin));
            if (gameRound.yourMove == Move.Paper)
                return PaperScore + (enemyMove == Move.Rock ? ScoreForAWin : (enemyMove == Move.Paper ? ScoreForADraw : ScoreForALoss));
            if (gameRound.yourMove == Move.Scissors)
                return ScissorsScore + (enemyMove == Move.Rock ? ScoreForALoss : (enemyMove == Move.Paper ? ScoreForAWin : ScoreForADraw));

            throw new ArgumentException("The input was somehow wrong.");
        }

        public static string SolvePart2(string inputFilePath)
        {
            List<string> commands = System.IO.File.ReadLines(inputFilePath).ToList();
            int totalScoreOfYou = 0;
            foreach (string command in commands)
                totalScoreOfYou += CalculcateYourScoreOfOneGameRoundAfterTheElfActuallyBotheredToExplainToYouHowItReallyWorks(StringToGameRoundPart2(command));
            return $"Your actual total score will be {totalScoreOfYou}"; // 12586
        }

        private static int CalculcateYourScoreOfOneGameRoundAfterTheElfActuallyBotheredToExplainToYouHowItReallyWorks(GameRoundForPart2 gameRound)
        {
            var enemyMove = gameRound.enemyMove;

            if (gameRound.gameResult == GameResult.Win)
                return ScoreForAWin + (enemyMove == Move.Scissors ? RockScore : (enemyMove == Move.Rock ? PaperScore : ScissorsScore));
            if (gameRound.gameResult == GameResult.Loose)
                return ScoreForALoss + (enemyMove == Move.Rock ? ScissorsScore : (enemyMove == Move.Paper ? RockScore : PaperScore));
            if (gameRound.gameResult == GameResult.Draw)
                return ScoreForADraw + (enemyMove == Move.Rock ? RockScore : (enemyMove == Move.Paper ? PaperScore : ScissorsScore));

            throw new ArgumentException("The input was somehow wrong.");
        }
    }
}

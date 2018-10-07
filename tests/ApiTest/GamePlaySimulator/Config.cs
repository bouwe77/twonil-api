namespace ApiTest.GamePlaySimulator
{
    public class Config
    {
        public Config(string gameId, int howManySeasons, bool checkLeagueTables)
        {
            GameId = gameId;
            HowManySeasons = howManySeasons;
            CheckLeagueTables = checkLeagueTables;
        }

        public string GameId { get; private set; }
        public int HowManySeasons { get; private set; }
        public bool CheckLeagueTables { get; private set; }
    }
}
namespace CoopChaos
{
    public class GameContext
    {        
        public static GameContext Singleton { get; } = new GameContext();

        public int MaxUserCount { get; }
        public int MinUserCount { get; }
        
        private GameContext()
        {
            MaxUserCount = 8;
            MinUserCount = 4;
        }
    }
}
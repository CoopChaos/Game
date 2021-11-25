namespace CoopChaos.Shared
{
    public class GameContext
    {        
        public static GameContext Singleton { get; } = new GameContext();

        public int MaxPlayerCount { get; }
        
        private GameContext()
        {
            MaxPlayerCount = 8;
        }
    }
}
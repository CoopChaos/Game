namespace CoopChaos
{
    public class ServerGameContext
    {        
        public static ServerGameContext Singleton { get; } = new ServerGameContext();

        public int MaxUserCount { get; }
        public int MinUserCount { get; }
        
        private ServerGameContext()
        {
            MaxUserCount = 8;
            MinUserCount = 4;
        }
    }
}
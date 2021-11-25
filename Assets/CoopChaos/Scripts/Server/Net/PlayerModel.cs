namespace CoopChaos
{
    public class PlayerModel
    {
        public string Username { get; }
        public ulong ClientId { get; }

        public PlayerModel(string username, ulong clientId)
        {
            Username = username;
            ClientId = clientId;
        }
    }
}
using System;

namespace CoopChaos
{
    public class UserModel
    {
        
        public string Username { get; }
        public ulong ClientId { get; }

        public UserModel(string username, ulong clientId)
        {
            Username = username;
            ClientId = clientId;
        }
    }
}
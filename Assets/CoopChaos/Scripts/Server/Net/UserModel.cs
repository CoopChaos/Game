using System;

namespace CoopChaos
{
    public class UserModel
    {
        public UserModel(string username, ulong clientId)
        {
            Username = username;
            ClientId = clientId;
        }
        
        public string Username { get; }
        public ulong ClientId { get; }
    }
}
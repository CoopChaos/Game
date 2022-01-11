using System;

namespace Yame
{
    public class ServerUserModel
    {
        public ServerUserModel(string username, ulong clientId, Guid clientHash)
        {
            Username = username;
            ClientId = clientId;
            ClientHash = clientHash;
        }

        public string Username { get; }
        public ulong ClientId { get; }
        public Guid ClientHash { get; }
    }
}
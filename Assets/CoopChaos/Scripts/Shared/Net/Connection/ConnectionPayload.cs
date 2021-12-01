using System;

namespace CoopChaos
{
    [Serializable]
    public class ConnectionPayload
    {
        public const int MaxUsernameLength = 20;
        
        // semi secret token allowing to identify the client
        // after reconnect and assign it to the correct player
        public Guid Token;
        
        public string Username;
        
        public bool Verify()
            => string.IsNullOrWhiteSpace(Username) == false && Username.Length <= MaxUsernameLength && Token != Guid.Empty;
    }
}
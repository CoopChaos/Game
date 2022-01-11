using System;

namespace Yame
{
    [Serializable]
    public class ConnectionPayload
    {
        public const int MaxUsernameLength = 20;
        
        // semi secret token allowing to identify the client
        // after reconnect and assign it to the correct player
        public string RawToken;
        public string Username;

        public Guid Token => new Guid(RawToken);
        
        public bool Verify()
            => string.IsNullOrWhiteSpace(Username) == false 
               && Username.Length <= MaxUsernameLength 
               && !string.IsNullOrWhiteSpace(RawToken) 
               && Token != Guid.Empty;
    }
}
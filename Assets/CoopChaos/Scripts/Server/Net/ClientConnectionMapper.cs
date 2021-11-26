using System;
using System.Collections.Generic;

namespace CoopChaos
{
    public class ClientConnectionMapper
    {
        private Dictionary<ulong, Guid> clientIdToClientToken = new Dictionary<ulong, Guid>();

        public Guid this[ulong connectionId]
        {
            get
            {
                clientIdToClientToken.TryGetValue(connectionId, out var clientId);
                return clientId;
            }
        }
        
        
        public void Add(Guid clientId, ulong connectionId)
        {
            clientIdToClientToken.Add(connectionId, clientId);
        }
        
        public bool TryGetClientId(ulong connectionId, out Guid clientId)
        {
            return clientIdToClientToken.TryGetValue(connectionId, out clientId);
        }

        public bool ContainsKey(ulong connectionId)
        {
            return clientIdToClientToken.ContainsKey(connectionId);
        }

        public void Remove(ulong connectionId)
        {
            clientIdToClientToken.Remove(connectionId);
        }

        public void Clear()
        {
            clientIdToClientToken.Clear();
        }
    }
}
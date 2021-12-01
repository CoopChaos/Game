using System;
using System.Collections.Generic;
using System.Linq;

namespace CoopChaos
{
    public class UserConnectionMapper
    {
        private Dictionary<ulong, Guid> clientIdToClientToken = new Dictionary<ulong, Guid>();
        
        public int Count => clientIdToClientToken.Count;

        public Guid this[ulong clientId]
        {
            get
            {
                clientIdToClientToken.TryGetValue(clientId, out var tokenHash);
                return tokenHash;
            }
        }

        public ulong this[Guid tokenHash]
            => clientIdToClientToken.FirstOrDefault(x => x.Value == tokenHash).Key;
        
        public void Add(Guid tokenHash, ulong clientId)
        {
            clientIdToClientToken.Add(clientId, tokenHash);
        }
        
        public bool TryGetClientId(ulong clientId, out Guid tokenHash)
        {
            return clientIdToClientToken.TryGetValue(clientId, out tokenHash);
        }

        public bool Contains(Guid tokenHash)
        {
            return clientIdToClientToken.ContainsValue(tokenHash);
        }

        public void Remove(ulong clientId)
        {
            clientIdToClientToken.Remove(clientId);
        }

        public void Clear()
        {
            clientIdToClientToken.Clear();
        }
    }
}
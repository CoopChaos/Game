using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(ConnectionManager))]
    public class UserConnectionMapper : NetworkBehaviour
    {
        private NetworkList<Connection> connections = new NetworkList<Connection>();
        public static UserConnectionMapper Singleton { get; private set; }

        public override void OnNetworkSpawn()
        {
            connections.Clear();
            
            if (IsServer)
            {
                foreach (var connection in NetworkManager.Singleton.ConnectedClientsList)
                {
                    var user = connection.PlayerObject.GetComponent<UserPersistentBehaviour>();
                    connections.Add(new Connection(user.UserModel.ClientId, user.UserModel.ClientHash));
                }
                
                // userconnectionmapper is manually handled by connectionmapper to
                // prevent racing conditions casued by client connect and disconnect events
            }
        }

        private void Start()
        {
            Assert.IsNull(Singleton);
            Singleton = this;
            // DontDestroyOnLoad(gameObject); ist already ensured by ConnectionManager
        }

        public override void OnDestroy()
        {
            Singleton = null;
        }

        public int Count
            => connections.Count;

        public Guid this[ulong clientId]
            => connections.First(c => c.ClientId == clientId).ClientHash;

        public ulong this[Guid clientHash]
            => connections.First(x => x.ClientHash == clientHash).ClientId;
        
        public void Add(Guid clientHash, ulong clientId)
        {
            connections.Add(new Connection(clientId, clientHash));
        }
        
        public void Remove(Guid clientHash)
        {
            connections.Remove(connections.First(c => c.ClientHash == clientHash));
        }
        
        public bool TryGetClientId(ulong clientId, out Guid clientHash)
        {
            foreach (var connection in connections)
            {
                if (connection.ClientId == clientId)
                {
                    clientHash = connection.ClientHash;
                    return true;
                }
            }
            
            clientHash = Guid.Empty;
            return false;
        }

        public bool Contains(Guid clientHash) 
            => connections.IndexWhere(u => u.ClientHash == clientHash) != -1;

        public void Remove(ulong clientId)
        {
            var index = connections.IndexWhere(c => c.ClientId == clientId);
            if (index != -1)
            {
                connections.RemoveAt(index);
            }
        }

        public void Clear()
            => connections.Clear();

        private struct Connection : INetworkSerializable, IEquatable<Connection>
        {
            private ulong clientId;
            private Guid clientHash;

            public Connection(ulong clientId, Guid clientHash)
            {
                this.clientId = clientId;
                this.clientHash = clientHash;
            }
            
            public ulong ClientId => clientId;
            public Guid ClientHash => clientHash;

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref clientId);
                serializer.SerializeValue(ref clientHash);
            }

            public bool Equals(Connection other)
            {
                return clientId == other.clientId && clientHash.Equals(other.clientHash);
            }

            public override bool Equals(object obj)
            {
                return obj is Connection other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (clientId.GetHashCode() * 397) ^ clientHash.GetHashCode();
                }
            }
        }
    }
}
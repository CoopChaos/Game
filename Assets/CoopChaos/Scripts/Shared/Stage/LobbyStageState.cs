using System;
using Unity.Netcode;

namespace CoopChaos
{
    public class LobbyStageState : Stage
    {
        private NetworkList<UserModel> users = new NetworkList<UserModel>();

        public event Action<Guid> OnToggleUserReady;

        public event Action<Guid, bool> OnUserReadyChanged;
        public event Action<UserModel> OnUserConnected;
        public event Action<Guid> OnUserDisconnected;
        
        public NetworkList<UserModel> Users => users;
        public override StageType Type => StageType.Lobby;
        
        public void ToggleUserReady(Guid clientHash)
        {
            OnToggleUserReady?.Invoke(clientHash);
        }

        [ClientRpc]
        public void UserReadyChangedClientRpc(Guid clientHash)
            => OnUserReadyChanged?.Invoke(clientHash, users[users.IndexWhere(u => u.ClientHash == clientHash)].Ready);

        [ClientRpc]
        public void UserConnectedClientRpc(Guid clientHash) 
            => OnUserConnected?.Invoke(users[users.IndexWhere(u => u.ClientHash == clientHash)]);

        [ClientRpc]
        public void UserDisconnectedClientRpc(Guid clientHash)
            => OnUserDisconnected?.Invoke(clientHash);

        public struct UserModel : INetworkSerializable, IEquatable<UserModel>
        {
            private Guid clientHash;

            private bool ready;
            private NetworkString username;

            public UserModel(Guid clientHash, bool ready, NetworkString username)
            {
                this.clientHash = clientHash;
                this.ready = ready;
                this.username = username;
            }

            public Guid ClientHash => clientHash;
            public bool Ready => ready;
            public string Username => username.ToString();
            public NetworkString RawUsername => username;
            
            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref clientHash);
                serializer.SerializeValue(ref ready);
                serializer.SerializeValue(ref username);
            }

            public bool Equals(UserModel other)
            {
                return clientHash == other.clientHash && ready == other.ready && username.Equals(other.username);
            }

            public override bool Equals(object obj)
            {
                return obj is UserModel other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = clientHash.GetHashCode();
                    hashCode = (hashCode * 397) ^ ready.GetHashCode();
                    hashCode = (hashCode * 397) ^ username.GetHashCode();
                    return hashCode;
                }
            }
        }
    }
}
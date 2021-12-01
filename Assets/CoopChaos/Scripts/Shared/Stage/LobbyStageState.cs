using System;
using Unity.Netcode;

namespace CoopChaos
{
    public class LobbyStageState : Stage
    {
        private NetworkList<UserModel> users = new NetworkList<UserModel>();

        public event Action<ulong> OnToggleUserReady;

        public event Action<ulong> OnUserReadyChanged;
        public event Action<ulong> OnUserConnected;
        public event Action<ulong> OnUserDisconnected;
        
        public NetworkList<UserModel> Users => users;
        public override StageType Type => StageType.Lobby;
        
        public void ToggleUserReady(ulong clientId)
        {
            OnToggleUserReady?.Invoke(clientId);
        }

        [ClientRpc]
        public void UserReadyChangedClientRpc(ulong clientId)
            => OnUserReadyChanged?.Invoke(clientId);

        [ClientRpc]
        public void UserConnectedClientRpc(ulong clientId) 
            => OnUserConnected?.Invoke(clientId);

        [ClientRpc]
        public void UserDisconnectedClientRpc(ulong clientId)
            => OnUserDisconnected?.Invoke(clientId);

        public struct UserModel : INetworkSerializable, IEquatable<UserModel>
        {
            private ulong clientId;
            
            private bool ready;
            private NetworkString username;

            public UserModel(ulong clientId, bool ready, NetworkString username)
            {
                this.clientId = clientId;
                this.ready = ready;
                this.username = username;
            }

            public ulong ClientId => clientId;
            public bool Ready => ready;
            public string Username => username.ToString();
            public NetworkString RawUsername => username;
            
            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref clientId);
                serializer.SerializeValue(ref ready);
                serializer.SerializeValue(ref username);
            }

            public bool Equals(UserModel other)
            {
                return clientId == other.clientId && ready == other.ready && username.Equals(other.username);
            }

            public override bool Equals(object obj)
            {
                return obj is UserModel other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = clientId.GetHashCode();
                    hashCode = (hashCode * 397) ^ ready.GetHashCode();
                    hashCode = (hashCode * 397) ^ username.GetHashCode();
                    return hashCode;
                }
            }
        }
    }
}
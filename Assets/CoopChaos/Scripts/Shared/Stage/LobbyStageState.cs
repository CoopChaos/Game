using System;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    [RequireComponent(typeof(ClientLobbyStage), typeof(ServerLobbyStage))]
    public class LobbyStageState : Stage
    {
        private CustomNetworkList<UserModel> users;

        public CustomNetworkList<UserModel> Users => users;
        public override StageType Type => StageType.Lobby;

        protected void Awake()
        {
            users = new CustomNetworkList<UserModel>();
        }

        public struct UserModel : INetworkSerializable, IEquatable<UserModel>
        {
            private ulong id;
            private Guid clientHash;

            private bool ready;
            private NetworkString username;

            public UserModel(ulong id, Guid clientHash, bool ready, NetworkString username)
            {
                this.id = id;
                this.clientHash = clientHash;
                this.ready = ready;
                this.username = username;
            }

            public ulong Id => id;
            public Guid ClientHash => clientHash;
            public bool Ready => ready;
            public string Username => username.ToString();
            public NetworkString RawUsername => username;
            
            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref id);
                serializer.SerializeValue(ref clientHash);
                serializer.SerializeValue(ref ready);
                serializer.SerializeValue(ref username);
            }

            public bool Equals(UserModel other)
            {
                return id == other.id && clientHash == other.clientHash;
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
                    hashCode = (hashCode * 397) ^ id.GetHashCode();
                    hashCode = (hashCode * 397) ^ ready.GetHashCode();
                    hashCode = (hashCode * 397) ^ username.GetHashCode();
                    return hashCode;
                }
            }
        }
    }
}
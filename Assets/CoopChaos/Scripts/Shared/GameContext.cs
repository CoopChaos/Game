using System;
using Unity.Netcode;

namespace CoopChaos.Shared
{
    public struct GameContext : INetworkSerializable, IEquatable<GameContext>
    {
        private int maxUserCount;
        private int minUserCount;
        
        public static GameContext Default => new GameContext(8, 1);

        public GameContext(int maxUserCount, int minUserCount)
        {
            this.maxUserCount = maxUserCount;
            this.minUserCount = minUserCount;
        }

        public int MaxUserCount => maxUserCount;
        public int MinUserCount => minUserCount;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref maxUserCount);
            serializer.SerializeValue(ref minUserCount);
        }

        public bool Equals(GameContext other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return maxUserCount == other.maxUserCount && minUserCount == other.minUserCount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GameContext) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (maxUserCount * 397) ^ minUserCount;
            }
        }
    }
}
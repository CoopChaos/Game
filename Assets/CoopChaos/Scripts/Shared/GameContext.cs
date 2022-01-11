using System;
using Unity.Netcode;

namespace Yame
{
    public struct GameContext : INetworkSerializable, IEquatable<GameContext>
    {
        private int maxUserCount;
        private int minUserCount;
        private float interactRange;
        
        public static GameContext Singleton => new GameContext(8, 1, 3f);

        public GameContext(int maxUserCount, int minUserCount, float interactRange)
        {
            this.maxUserCount = maxUserCount;
            this.minUserCount = minUserCount;
            this.interactRange = interactRange;
        }

        public int MaxUserCount => maxUserCount;
        public int MinUserCount => minUserCount;
        public float InteractRange => interactRange;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref maxUserCount);
            serializer.SerializeValue(ref minUserCount);
            serializer.SerializeValue(ref interactRange);
        }

        public bool Equals(GameContext other)
        {
            return maxUserCount == other.maxUserCount && minUserCount == other.minUserCount && interactRange.Equals(other.interactRange);
        }
    }
}
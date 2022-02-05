using System;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    [Serializable]
    public struct GameContext : INetworkSerializable, IEquatable<GameContext>
    {
        [SerializeField]
        private int maxUserCount;

        [SerializeField]
        private int minUserCount;

        [SerializeField]
        private float interactRange;
        
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
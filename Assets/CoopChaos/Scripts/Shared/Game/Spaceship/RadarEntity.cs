using System;
using Unity.Netcode;

namespace CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship
{
    public struct RadarEntity : INetworkSerializable, IEquatable<RadarEntity>
    {
        private Guid uid;
        private int x;
        private int y;
        private EntityType type;

        public enum EntityType
        {
            Player,
            Enemy,
            Asteroid,
            Bullet
        }
        public RadarEntity(int x, int y, EntityType type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
            this.uid = Guid.NewGuid();
        }

        public Guid Uid => uid;
        public int X => x;
        public int Y => y;
        public EntityType Type => type;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref x);
            serializer.SerializeValue(ref y);
            serializer.SerializeValue(ref type);
        }
        
        public bool Equals(RadarEntity other)
        {
            return x == other.x && y == other.y && type == other.type;
        }        
        
    }
}
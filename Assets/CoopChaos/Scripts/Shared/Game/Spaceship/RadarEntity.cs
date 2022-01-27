using System;
using Unity.Netcode;

namespace CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship
{
    public struct RadarEntity : INetworkSerializable, IEquatable<RadarEntity>
    {
        private Guid uid;
        private float x;
        private float y;
        private EntityType type;

        public enum EntityType
        {
            Spaceship,
            Asteroid,
            Bullet
        }
        public RadarEntity(int x, int y, EntityType type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
            uid = Guid.NewGuid();
        }

        public Guid Uid => uid;
        public float X => x;
        public float Y => y;
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
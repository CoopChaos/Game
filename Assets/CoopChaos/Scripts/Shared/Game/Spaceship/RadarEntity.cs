using System;
using CoopChaos.Simulation.Components;
using Unity.Netcode;

namespace CoopChaos
{
    public struct 
    RadarEntity : INetworkSerializable, IEquatable<RadarEntity>
    {
        private Guid uid;
        private float x;
        private float y;
        private DetectionType type;

        public RadarEntity(float x, float y, DetectionType type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
            uid = Guid.NewGuid();
        }

        public Guid Uid => uid;
        public float X => x;
        public float Y => y;
        public DetectionType Type => type;

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
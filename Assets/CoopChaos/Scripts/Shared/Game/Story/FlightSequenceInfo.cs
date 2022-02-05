using System;
using CoopChaos;
using Unity.Collections;
using Unity.Netcode;

namespace DefaultNamespace
{
    public class FlightSequenceInfo : IEquatable<FlightSequenceInfo>, INetworkSerializable
    {
        private FixedBytes510 description;

        public string Description => description.ToString();

        public bool Equals(FlightSequenceInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return description.Equals(other.description);
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref description);
        }
    }
}
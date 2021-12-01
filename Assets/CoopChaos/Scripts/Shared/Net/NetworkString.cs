using Unity.Collections;
using Unity.Netcode;

namespace CoopChaos
{
    // 32 character network string
    public struct NetworkString : INetworkSerializable
    {
        private FixedString32Bytes value;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref value);
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public static implicit operator string(NetworkString s) => s.ToString();
        public static implicit operator NetworkString(string s) => new NetworkString() { value = new FixedString32Bytes(s) };
    }
}
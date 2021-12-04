using Unity.Collections;
using Unity.Netcode;

namespace CoopChaos
{
    public static class CustomMessagingHelper
    {
        public static CustomMessage StartSend() => new CustomMessage();

        public class CustomMessage
        {
            private FastBufferWriter writer;
            
            public CustomMessage()
            {
                writer = new FastBufferWriter(sizeof(ConnectResult), Allocator.Temp);
            }
            
            public CustomMessage Write<T>(T value) where T : unmanaged
            {
                writer.WriteValueSafe(value);
                return this;
            }
            
            public CustomMessage Send(ulong clientId, NetworkMessage networkMessage)
            {
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(networkMessage.ToString(), clientId, writer);
                return this;
            }
        }
    }
}
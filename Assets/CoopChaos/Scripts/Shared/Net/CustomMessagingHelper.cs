using Unity.Collections;
using Unity.Netcode;

namespace CoopChaos.Shared
{
    
    /*
     * 
        public void SendMessage<T>(ulong clientId, T value, NetworkMessage networkMessage) where T : unmanaged
        {
            var writer = new FastBufferWriter(sizeof(ConnectStatus), Allocator.Temp);
            writer.WriteValueSafe(value);
            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(networkMessage.ToString(), clientId, writer);
        }
     */
    
    public static class CustomMessagingHelper
    {
        public static CustomMessage CreateSend() => new CustomMessage();

        public class CustomMessage
        {
            private FastBufferWriter writer;
            
            public CustomMessage()
            {
                writer = new FastBufferWriter(sizeof(ConnectStatus), Allocator.Temp);
            }
            
            public CustomMessage Write<T>(T value) where T : unmanaged
            {
                writer.WriteValueSafe(value);
                return this;
            }
            
            public CustomMessage Send(ulong clientId, NetworkMessage networkMessage) where T : unmanaged
            {
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(networkMessage.ToString(), clientId, writer);
                return this;
            }
        }
    }
}
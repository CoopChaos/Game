using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class ClientConnectionManager : MonoBehaviour
    {
        public void OnNetworkReady()
        {
            if (!NetworkManager.Singleton.IsClient)
            {
                enabled = false;
            }
        }
        
        private void ConnectClient(string ip, int port)
        {
            // ...

            NetworkManager.Singleton.StartClient();
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(nameof(NetworkMessage.DisconnectReason), OnDisconnectReasonReceived);
        }
        
        private void OnDisconnectReasonReceived(ulong clientID, FastBufferReader reader)
        {
            reader.ReadValueSafe(out ConnectStatus status);
            
            // ...
        }
    }
}
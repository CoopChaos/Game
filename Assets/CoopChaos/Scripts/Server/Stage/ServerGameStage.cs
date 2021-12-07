using Unity.Netcode;

namespace CoopChaos
{
    public class ServerGameStage : Stage
    {
        public override StageType Type => StageType.Game;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                enabled = false;
                return;
            }

            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientDisconnected;
        }

        public override void OnNetworkDespawn()
        {
            UnregisterCallbacks();   
        }

        protected override void OnDestroy()
        {
            UnregisterCallbacks();
        }

        private void UnregisterCallbacks()
        {
            if (!IsServer)
                return;
            
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientDisconnected;
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
        }
        
        private void HandleClientDisconnected(ulong clientId)
        {
        }
    }
}
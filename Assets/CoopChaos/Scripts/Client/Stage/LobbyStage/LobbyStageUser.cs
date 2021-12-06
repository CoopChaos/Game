using Unity.Netcode;

namespace CoopChaos
{
    public class LobbyStageUser : NetworkBehaviour
    {
        private LobbyStageState state;
        
        public override void OnNetworkSpawn()
        {
            if (!IsClient)
            {
                enabled = false;
                return;
            }
            
            
        }

        [ServerRpc]
        public void ToggleReady()
        {
            
        }
    }
}
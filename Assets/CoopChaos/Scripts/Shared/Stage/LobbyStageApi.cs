using System;
using Unity.Netcode;

namespace CoopChaos
{
    public class LobbyStageApi : NetworkBehaviour
    {
        private LobbyStageState lobbyStageState;

        [ServerRpc]
        public void ToggleReadyServerRpc()
        {
            lobbyStageState.ToggleUserReady(UserConnectionMapper.Singleton[OwnerClientId]);
        }

        private void Start()
        {
            lobbyStageState = GetComponent<LobbyStageState>();
        }
    }
}
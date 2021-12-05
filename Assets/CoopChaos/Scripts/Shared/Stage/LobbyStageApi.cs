using System;
using Unity.Netcode;

namespace CoopChaos
{
    public class LobbyStageApi : NetworkBehaviour
    {
        private LobbyStageState lobbyStageState;

        [ServerRpc(RequireOwnership = false)]
        public void ToggleReadyServerRpc(Guid clientToken)
        {
            lobbyStageState.ToggleUserReady(UserConnectionMapper.TokenToClientHash(clientToken));
        }

        private void Start()
        {
            lobbyStageState = GetComponent<LobbyStageState>();
        }
    }
}
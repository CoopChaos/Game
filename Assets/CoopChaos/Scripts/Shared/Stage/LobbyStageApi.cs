using Unity.Netcode;

namespace CoopChaos
{
    public class LobbyStageApi : NetworkBehaviour
    {
        private LobbyStageState lobbyStageState;

        public LobbyStageApi(LobbyStageState lobbyStageState)
        {
            this.lobbyStageState = lobbyStageState;
        }

        [ServerRpc]
        public void ToggleReadyServerRpc()
        {
            lobbyStageState.ToggleUserReady(OwnerClientId);
        }
    }
}
using System;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class LobbyStageUserApi : NetworkBehaviour
    {
        private ServerLobbyStage serverLobbyStage;
        
        public override void OnNetworkSpawn()
        {
        }

        [ServerRpc(RequireOwnership = true)]
        public void ToggleReadyServerRpc()
        {
            serverLobbyStage.ToggleUserReady(OwnerClientId);
        }

        private void Awake()
        {
            serverLobbyStage = FindObjectOfType<ServerLobbyStage>();
        }
    }
}
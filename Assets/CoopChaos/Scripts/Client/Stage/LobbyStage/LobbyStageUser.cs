using System;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public class LobbyStageUser : NetworkBehaviour
    {
        private LobbyStageState state;
        
        public override void OnNetworkSpawn()
        {
        }

        [ServerRpc(RequireOwnership = true)]
        public void ToggleReadyServerRpc()
        {
            state.ToggleUserReady(UserConnectionMapper.Singleton[OwnerClientId]);
        }

        private void Awake()
        {
            state = FindObjectOfType<LobbyStageState>();
        }
    }
}
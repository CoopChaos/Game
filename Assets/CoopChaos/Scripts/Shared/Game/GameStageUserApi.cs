using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    // the object of this component is owned by the player
    public class GameStageUserApi : NetworkBehaviour
    {
        private ServerGameStage serverGameStage;

        public override void OnNetworkSpawn()
        {
        }

        [ServerRpc]
        public void InteractServerRpc(ulong interactableObjectId)
        {
            serverGameStage.InteractWith(OwnerClientId, interactableObjectId);
        }

        private void Start()
        {
            serverGameStage = FindObjectOfType<ServerGameStage>();
            Assert.IsNotNull(serverGameStage);
        }
    }
}
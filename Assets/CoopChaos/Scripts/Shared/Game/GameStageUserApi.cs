using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace Yame
{
    // the object of this component is owned by the player
    public class GameStageUserApi : NetworkBehaviour
    {
        private ServerSpaceship serverSpaceship;

        public override void OnNetworkSpawn()
        {
        }

        [ServerRpc]
        public void InteractServerRpc(ulong interactableObjectId)
        {
            serverSpaceship.InteractWith(OwnerClientId, interactableObjectId);
        }

        private void Start()
        {
            serverSpaceship = FindObjectOfType<ServerSpaceship>();
            Assert.IsNotNull(serverSpaceship);
        }
    }
}
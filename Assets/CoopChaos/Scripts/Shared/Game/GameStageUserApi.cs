using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    // the object of this component is owned by the player
    public class GameStageUserApi : NetworkBehaviour
    {
        private ServerSpaceship serverSpaceship;

        public override void OnNetworkSpawn()
        {
            serverSpaceship = FindObjectOfType<ServerSpaceship>();
            Assert.IsNotNull(serverSpaceship);
        }

        [ServerRpc]
        public void Interact(int interactableObjectId)
        {
            serverSpaceship.InteractFromPlayer(OwnerClientId, interactableObjectId);
        }
    }
}
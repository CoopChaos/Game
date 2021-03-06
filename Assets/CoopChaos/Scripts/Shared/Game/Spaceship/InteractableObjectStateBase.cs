using System;
using Unity.Netcode;
using UnityEngine.Assertions;

namespace CoopChaos
{
    public abstract class InteractableObjectStateBase : NetworkBehaviour
    {
        private static int interactableObjectIdCounter = 1;
        private int InteractableObjectId { get; set; }

        public event Action<ulong> InteractEvent;

        [ClientRpc]
        public void InteractClientRpc(ulong clientId)
        {
            InteractEvent?.Invoke(clientId);
        }

        public override void OnNetworkSpawn()
        {
            Assert.IsTrue(InteractableObjectId != 0, "InteractableObjectId must be set before OnNetworkSpawn");
        }

        protected virtual void Awake()
        {
            InteractableObjectId = interactableObjectIdCounter++;
        }
    }
}
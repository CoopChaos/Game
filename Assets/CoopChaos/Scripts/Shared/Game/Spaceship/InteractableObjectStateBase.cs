using System;
using Unity.Netcode;
using UnityEngine.Assertions;

namespace CoopChaos
{
    public abstract class InteractableObjectStateBase : NetworkBehaviour
    {
        private static int interactableObjectIdCounter = 0;
        public int InteractableObjectId { get; private set; } = 0;

        public abstract void Interact(ulong clientId);
        
        public override void OnNetworkSpawn()
        {
            Assert.IsTrue(InteractableObjectId != 0, "InteractableObjectId must be set before OnNetworkSpawn");
        }

        private void Awake()
        {
            InteractableObjectId = interactableObjectIdCounter++;
        }
    }
}
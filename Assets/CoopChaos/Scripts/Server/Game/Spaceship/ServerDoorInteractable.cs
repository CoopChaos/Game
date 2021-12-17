using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(DoorInteractableState))]
    public class ServerDoorInteractable : ServerInteractableObjectBase
    {
        private DoorInteractableState doorInteractableState;
        
        public override void Interact(ulong clientId)
        {
            doorInteractableState.Open.Value = !doorInteractableState.Open.Value;
        }

        private void Awake()
        {
            doorInteractableState = GetComponent<DoorInteractableState>();
            Assert.IsNotNull(doorInteractableState);
        }
    }
}
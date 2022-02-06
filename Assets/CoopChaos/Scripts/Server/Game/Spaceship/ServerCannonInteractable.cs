using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(CannonInteractableState))]
    public class ServerCannonInteractable : ServerInteractableObjectBase
    {
        private CannonInteractableState interactableState;
        public override void Interact(ulong clientId)
        {
            
        }
        
        private void Awake()
        {
            interactableState = GetComponent<CannonInteractableState>();
            Assert.IsNotNull(interactableState);
        }

        protected override void Start()
        {
            base.Start();
        }

    }
}

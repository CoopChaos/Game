using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(SpaceshipControlInteractableState))]
    public class ServerSpaceshipControlInteractable : ServerInteractableObjectBase
    {
        private SpaceshipControlInteractableState interactableState;
        public override void Interact(ulong clientId)
        {
            
        }
        
        private void Awake()
        {
            interactableState = GetComponent<SpaceshipControlInteractableState>();
            Assert.IsNotNull(interactableState);
        }

        protected override void Start()
        {
            base.Start();
        }

    }
}

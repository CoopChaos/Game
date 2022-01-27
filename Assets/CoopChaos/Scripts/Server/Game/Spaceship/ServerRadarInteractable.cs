using System.Collections;
using System.Collections.Generic;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(RadarInteractableState))]
    public class ServerRadarInteractable : ServerInteractableObjectBase
    {
        private RadarInteractableState interactableState;
        public override void Interact(ulong clientId)
        {
            
        }
        
        private void Awake()
        {
            interactableState = GetComponent<RadarInteractableState>();
            Assert.IsNotNull(interactableState);
        }

        protected override void Start()
        {
            base.Start();
        }

    }
}
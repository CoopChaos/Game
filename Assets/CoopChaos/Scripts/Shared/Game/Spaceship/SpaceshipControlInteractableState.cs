using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    [RequireComponent(typeof(ServerSpaceshipControlInteractable), typeof(ClientSpaceshipControlInteractable))]
    public class SpaceshipControlInteractableState : InteractableObjectStateBase
    {
        private NetworkVariable<bool> isBlocked;

        public NetworkVariable<bool> IsBlocked => isBlocked;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            isBlocked.OnValueChanged += HandleIsBlockedChanged;
        }

        protected override void Awake()
        {
            base.Awake();
            
            isBlocked = new NetworkVariable<bool>(false);
        }
        
        private void HandleIsBlockedChanged(bool blocked, bool oldBlocked)
        {
            // TODO: Show or don't show Spaceship Control
        }
    }
}

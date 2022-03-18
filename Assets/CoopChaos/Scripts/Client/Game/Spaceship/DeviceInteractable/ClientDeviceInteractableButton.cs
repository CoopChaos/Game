using System;
using CoopChaos;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;

namespace Yame
{
    public class ClientDeviceInteractableButton : ClientDeviceInteractableBase
    {
        private DeviceInteractableBaseState deviceInteractableState;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            deviceInteractableState.Claimed.OnValueChanged = HandleClaimChanged;
        }

        protected override void HandleClaimChanged(bool claim, bool oldClaim)
        {
            base.HandleClaimChanged(claim, oldClaim);
            deviceSprite.SetActive(false);
            deviceInteractableState.Fulfilled.Value = true;
            deviceInteractableState.Claimed.Value = false;
        }

        protected override void Awake()
        {
            base.Awake();

            // Hide Minigame from Player if role is not suitable
            if(deviceInteractableState.IsRoleBound && GetComponent<GameStageUserState>().Role.Value != deviceInteractableState.Role) {
                deviceSprite.SetActive(false);
                highlight.SetActive(false);
            }

            deviceInteractableState.InteractEvent += user =>
            {
                deviceInteractableState.Claimed.Value = true;
            };
        }
    }
}
using System;
using CoopChaos;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;
using UnityEngine.UI;

namespace Yame
{
    public class ClientDeviceInteractableMinigame : ClientDeviceInteractableBase
    {
        [SerializeField] private GameObject minigame;
        private BaseThreatMinigame baseThreatMinigame;

        private DeviceInteractableBaseState deviceInteractableState;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();


            deviceInteractableState.Claimed.OnValueChanged = HandleClaimChanged;
        }

        protected override void HandleClaimChanged(bool claim, bool oldClaim)
        {
            base.HandleClaimChanged(claim, oldClaim);
            baseThreatMinigame.StartMinigame();
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            deviceInteractableState = GetComponent<DeviceInteractableBaseState>();

            deviceInteractableState.InteractEvent += user =>
            {
                baseThreatMinigame.StartMinigame();
            };

            baseThreatMinigame = minigame.GetComponent<BaseThreatMinigame>();
        }

        public void Update()
        {
            if (claimedByMe && baseThreatMinigame.IsFinished())
            {
                deviceSprite.SetActive(false);
                deviceInteractableState.Fulfilled.Value = true;
                deviceInteractableState.Claimed.Value = false;
            }
            
        }
    }
}
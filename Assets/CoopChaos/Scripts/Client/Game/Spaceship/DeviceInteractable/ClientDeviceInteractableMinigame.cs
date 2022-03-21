using System;
using CoopChaos;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using Unity.Netcode;
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

            if (!IsClient)
            {
                enabled = false;
                return;
            }

            baseThreatMinigame = minigame.GetComponent<BaseThreatMinigame>();
            
            deviceInteractableState.InteractEvent += user =>
            {
                if (user == NetworkManager.Singleton.LocalClientId)
                    baseThreatMinigame.StartMinigame();
            };

            deviceInteractableState.Claimed.OnValueChanged = HandleClaimChanged;
        }

        protected override void HandleClaimChanged(bool claim, bool oldClaim)
        {
            base.HandleClaimChanged(claim, oldClaim);
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            deviceInteractableState = GetComponent<DeviceInteractableBaseState>();
        }

        public void Update()
        {
            if (claimedByMe && baseThreatMinigame.IsFinished())
            {
                deviceInteractableState.FulfillServerRpc();
            }
        }
    }
}
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
                    if(!baseThreatMinigame.IsOpen())
                        baseThreatMinigame.StartMinigame();
                    else
                        baseThreatMinigame.PauseMinigame();
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

        public override void Unhighlight()
        {
            base.Unhighlight();
            minigame.GetComponent<BaseThreatMinigame>().PauseMinigame();
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
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

            /*
            if(deviceInteractableState.IsRoleBound && NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<GameStageUserState>().Role.Value != deviceInteractableState.Role) {
                deviceSprite.SetActive(false);
                highlight.SetActive(false);
            }
            */

            deviceInteractableState.Claimed.OnValueChanged = HandleClaimChanged;
        }

        protected override void HandleClaimChanged(bool claim, bool oldClaim)
        {
            base.HandleClaimChanged(claim, oldClaim);

            deviceInteractableState.InteractEvent += user =>
            {
                baseThreatMinigame.StartMinigame();
            };

            baseThreatMinigame.StartMinigame();
            // Hide Minigame from Player if role is not suitable

            baseThreatMinigame = minigame.GetComponent<BaseThreatMinigame>();
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
                deviceSprite.SetActive(false);
            }
            
        }
    }
}
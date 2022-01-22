using System;
using CoopChaos;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;

namespace Yame
{
    [RequireComponent(typeof(DeviceInteractableState), typeof(InteractableObjectBehaviour))]
    public class ClientDeviceInteractableMinigame : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject deviceSprite;
        [SerializeField] private GameObject highlight;
        [SerializeField] private GameObject minigame;
        private BaseThreatMinigame baseThreatMinigame;

        private bool claimedByMe = false;
        
        private DeviceInteractableState deviceInteractableState;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            deviceInteractableState.Claimed.OnValueChanged = HandleClaimChanged;
        }

        private void HandleClaimChanged(bool claim, bool oldClaim)
        {
            baseThreatMinigame.StartMinigame();
        }
        
        private void HandleOnHighlight()
        {
            highlight.SetActive(true);
        }
        
        private void HandleOnUnhighlight()
        {
            highlight.SetActive(false);
        }

        protected override void Awake()
        {
            base.Awake();
            
            deviceInteractableState = GetComponent<DeviceInteractableState>();
            baseThreatMinigame = minigame.GetComponent<BaseThreatMinigame>();
            
            interactable.OnHighlight += HandleOnHighlight;
            interactable.OnUnhighlight += HandleOnUnhighlight;
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
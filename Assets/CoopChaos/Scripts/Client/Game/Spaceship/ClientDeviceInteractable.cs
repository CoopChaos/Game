using System;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;

namespace Yame
{
    [RequireComponent(typeof(DeviceInteractableState), typeof(InteractableObjectBehaviour))]
    public class ClientDeviceInteractable : ClientInteractableObjectBase
    {
        [SerializeField] private GameObject deviceSprite;
        [SerializeField] private GameObject highlight;

        private float timer = 0;
        private bool claimedByMe = false;
        
        private DeviceInteractableState deviceInteractableState;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            deviceInteractableState.Claimed.OnValueChanged = HandleClaimChanged;
        }

        private void HandleClaimChanged(bool claim, bool oldClaim)
        {
            if (true)
            {
                Debug.Log("Claimed");
            }
            
            deviceSprite.SetActive(!claim);
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
            
            interactable.OnHighlight += HandleOnHighlight;
            interactable.OnUnhighlight += HandleOnUnhighlight;
        }

        public void Update()
        {
            float timeToFulfill = deviceInteractableState.TimeToFulFill.Value;
            
            // handle timer start and running
            if (claimedByMe && timer < timeToFulfill)
            {
                timer += Time.deltaTime;
            }
            
            // handle task aborted
            if (!claimedByMe && timer > 0 && timer < timeToFulfill)
            {
                timer = 0;
            }
        }
    }
}
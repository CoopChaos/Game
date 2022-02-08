using System;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;
using UnityEngine.UI;

namespace Yame
{
    public class ClientDeviceInteractableTimer : ClientDeviceInteractableBase
    {
        private float timer = 0;
        [SerializeField] private Text clock;
        [SerializeField] private Canvas clockContainer;

        private DeviceInteractableTimerState deviceInteractableState;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            deviceInteractableState.Claimed.OnValueChanged = HandleClaimChanged;
        }
        
        protected override void Awake()
        {
            base.Awake();
            clockContainer.enabled = false;
            deviceInteractableState = GetComponent<DeviceInteractableTimerState>();

            deviceInteractableState.InteractEvent += user =>
            {
                deviceInteractableState.Claimed.Value = true;
            };
        }

        public void Update()
        {
            float timeToFulfill = deviceInteractableState.TimeToFulFill.Value;
            
            // handle timer start and running
            if (claimedByMe && timer < timeToFulfill)
            {
                clockContainer.enabled = true;
                clock.text = ((int)timeToFulfill - timer).ToString();
                
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
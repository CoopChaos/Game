using System;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;

namespace Yame
{
    [RequireComponent(typeof(DeviceInteractableState))]
    public class ServerDeviceInteractable : ServerInteractableObjectBase
    {
        public DeviceInteractableState deviceInteractableState;
        private float timer = 0;

        // TODO: check if role is suited to interact with the device
        
        public override void Interact(ulong clientId)
        {
            deviceInteractableState.Claimed.Value = !deviceInteractableState.Claimed.Value;
            if (deviceInteractableState.Claimed.Value)
            {
                deviceInteractableState.ClientId.Value = clientId;
            }
            else
            {
                deviceInteractableState.ClientId.Value = 0;
            }
        }

        public void Update()
        {
            bool claimed = deviceInteractableState.Claimed.Value;
            float timeToFulfill = deviceInteractableState.TimeToFulFill.Value;
            
            // handle timer start and running
            if (claimed && timer < timeToFulfill)
            {
                timer += Time.deltaTime;
            }

            // handle timer finished
            if (claimed && timer >= timeToFulfill)
            {
                deviceInteractableState.Fulfilled.Value = true;
                deviceInteractableState.Claimed.Value = false;
                Debug.Log("--- Server Timer Finished ---");
            }
            
            // handle task aborted
            if (!claimed && timer > 0 && timer < timeToFulfill)
            {
                deviceInteractableState.Claimed.Value = false;
                timer = 0;
            }
        }
        
        protected void Awake()
        {
            deviceInteractableState = GetComponent<DeviceInteractableState>();
        }
    }
}
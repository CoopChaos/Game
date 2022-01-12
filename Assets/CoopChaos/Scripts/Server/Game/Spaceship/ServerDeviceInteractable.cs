using System;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;

namespace Yame
{
    [RequireComponent(typeof(DeviceInteractableState))]
    public class ServerDeviceInteractable : ServerInteractableObjectBase
    {
        private DeviceInteractableState deviceInteractableState;
        private float timer = 0;

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
                deviceInteractableState.Fulfilled.Value = false;
            }
            
            // handle task aborted
            if (!claimed && timer > 0 && timer < timeToFulfill)
            {
                deviceInteractableState.Claimed.Value = false;
                timer = 0;
            }
        }
    }
}
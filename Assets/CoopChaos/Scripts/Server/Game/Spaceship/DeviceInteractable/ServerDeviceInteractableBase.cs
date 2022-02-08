using System;
using CoopChaos;
using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;
using UnityEngine.Assertions;

namespace Yame
{
    public class ServerDeviceInteractableBase : ServerInteractableObjectBase
    {
        private DeviceInteractableBaseState deviceInteractableState;

        public DeviceInteractableBaseState DeviceInteractableState => deviceInteractableState;

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

        protected override void Start()
        {
            base.Start();
            deviceInteractableState = GetComponent<DeviceInteractableBaseState>();
            Assert.IsNotNull(deviceInteractableState);
            
            deviceInteractableState.Fulfilled.OnValueChanged = ReactFulfilled;
        }

        private void ReactFulfilled(bool previousvalue, bool newvalue)
        {
            Debug.Log("SERVER noticed fulfillment");
        }
    }
}
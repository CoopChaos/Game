using CoopChaos.CoopChaos.Scripts.Shared.Game.Spaceship;
using UnityEngine;

namespace Yame
{
    [RequireComponent(typeof(DeviceInteractableState))]
    public class ServerDeviceInteractable : ServerInteractableObjectBase
    {
        private DeviceInteractableState deviceInteractableState;
        
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
    }
}
using System;
using Unity.Netcode;

namespace CoopChaos
{
    public class DoorState : InteractableObjectStateBase
    {
        public event Action<ulong> OnDoorOpened;
        
        public override void Interact(ulong clientId)
        {
            OnDoorOpened?.Invoke(clientId);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            // ...
        }
    }
}
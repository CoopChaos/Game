using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace CoopChaos
{
    [RequireComponent(typeof(DoorState), typeof(InteractableObjectBehaviour))]
    public class ClientDoor : ClientInteractableObjectBase
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            interactable.OnHighlight += HandleOnHighlight;
            interactable.OnUnhighlight += HandleOnUnhighlight;
        }
        
        private void HandleOnHighlight()
        {
            
        }
        
        private void HandleOnUnhighlight()
        {
            
        }
    }
}
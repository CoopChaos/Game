using Unity.Netcode;
using UnityEngine.Assertions;

namespace CoopChaos
{
    public class ClientInteractableObjectBase : NetworkBehaviour
    {
        protected InteractableObjectBehaviour interactable;

        public override void OnNetworkSpawn()
        {
            if (!IsClient)
            {
                enabled = false;
                return;
            }
            
            var state = GetComponent<InteractableObjectStateBase>();
            interactable = GetComponent<InteractableObjectBehaviour>();
            
            Assert.IsNotNull(state);
            Assert.IsNotNull(interactable);
            
            interactable.InteractableObjectId = state.InteractableObjectId;
        }
    }
}
using System;
using Unity.Netcode;
using UnityEngine.Assertions;

namespace Yame
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
            
            Assert.IsNotNull(state);
            Assert.IsNotNull(interactable);
            
            interactable.InteractableObjectId = state.InteractableObjectId;
        }

        protected virtual void Awake()
        {
            interactable = GetComponent<InteractableObjectBehaviour>();
        }
    }
}
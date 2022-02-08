using System;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public abstract class ServerInteractableObjectBase : NetworkBehaviour
    {
        private InteractableObjectStateBase state;
        
        public virtual void Interact(ulong clientId)
        {
            state.InteractClientRpc(clientId);
        }

        protected virtual void Start()
        {
            state = GetComponent<InteractableObjectStateBase>();
            FindObjectOfType<ServerGameStage>().RegisterInteractableObject(this);
        }
    }
}
using System;
using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public abstract class ServerInteractableObjectBase : NetworkBehaviour
    {
        private InteractableObjectStateBase state;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsServer) 
            {
                enabled = false;
                return;
            }

            state = GetComponent<InteractableObjectStateBase>();
            FindObjectOfType<ServerGameStage>().RegisterInteractableObject(this);
        }

        protected virtual void Start()
        {
            
        }

        public virtual void Interact(ulong clientId)
        {
            state.InteractClientRpc(clientId);
        }
    }
}
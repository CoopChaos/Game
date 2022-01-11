using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace Yame
{
    public abstract class ClientInteractableObjectBase : NetworkBehaviour
    {
        public virtual Vector2 InteractionPoint => gameObject.transform.position;

        public abstract void Highlight();
        public abstract void Unhighlight();

        public override void OnNetworkSpawn()
        {
            if (!IsClient)
            {
                enabled = false;
                return;
            }
            
            var state = GetComponent<InteractableObjectStateBase>();
            
            Assert.IsNotNull(state);
        }

        protected virtual void Awake()
        {
        }
    }
}
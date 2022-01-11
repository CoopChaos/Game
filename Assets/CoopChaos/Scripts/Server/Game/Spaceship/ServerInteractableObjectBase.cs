using System;
using Unity.Netcode;
using UnityEngine;

namespace Yame
{
    public abstract class ServerInteractableObjectBase : NetworkBehaviour
    {
        public abstract void Interact(ulong clientId);

        protected virtual void Start()
        {
            FindObjectOfType<ServerSpaceship>().RegisterInteractableObject(this);
        }
    }
}
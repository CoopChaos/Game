using Unity.Netcode;
using UnityEngine;

namespace CoopChaos
{
    public abstract class ServerInteractableObjectBase : NetworkBehaviour
    {
        public abstract void Interact(ulong clientId);
    }
}